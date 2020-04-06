using Leopard.AspNetCore.Autofac.Extensions;
using Leopard.AspNetCore.Middleware;
using Leopard.Configuration;
using Leopard.Template.WebAPI.BackgroundServices;
using Leopard.Template.WebAPI.Config;
using Leopard.Template.WebAPI.HttpClients;
using Leopard.Template.WebAPI.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Polly;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Reflection;
using RedLockNet.SERedis;
using System.Collections.Generic;
using System.Net;
using RedLockNet.SERedis.Configuration;
using RedLockNet;
using Microsoft.Extensions.ObjectPool;

namespace Leopard.Template.WebAPI
{
    public class Startup
    {
        public IHostingEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        private DefaultPooledObjectPolicy<LocalSystemClock> defaultPolicy = null;
        public static DefaultObjectPool<LocalSystemClock> defaultPool = null;

        private static string[] docs = new[] { "未分类", "Swagger组" };
        private RedLockFactory lockFactory = null;



        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Env = env;
            this.Configuration = configuration;
            this.Configuration.AddConfigurationGeterLocator();

            defaultPolicy = new DefaultPooledObjectPolicy<LocalSystemClock>();
            defaultPool = new DefaultObjectPool<LocalSystemClock>(defaultPolicy, 2);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache(options =>
            {
                // 缓存压缩比为 2%，每 5 分钟进行一次过期缓存的扫描，最大缓存空间大小限制为 1024
                options.CompactionPercentage = 0.02d;
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
                //options.SizeLimit = 1024;
            });

            services.AddIConfigurationGeter().AddConfigFinder();
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });
            services.AddResponseCaching();

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                       new CacheProfile()
                       {
                           Duration = 60
                       });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true

                    });
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (Env.IsDevelopment())
            {
                services.AddSwaggerGen(options =>
                {
                    foreach (var doc in docs)
                    {
                        options.SwaggerDoc(doc, new Info { Version = doc });
                    }

                    options.DocInclusionPredicate((docName, description) =>
                    {
                        description.TryGetMethodInfo(out MethodInfo mi);

                        var attr = mi.DeclaringType.GetCustomAttribute<ApiExplorerSettingsAttribute>();

                        if (attr != null)
                        {
                            return attr.GroupName == docName;
                        }
                        else
                        {
                            return docName == "未分类";
                        }
                    });
                    options.CustomSchemaIds(d => d.FullName);
                    // 为 Swagger JSON and UI设置xml文档注释路径
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath, true);
                });
            }

            // 选项模式
            services.Configure<MySetting>(this.Configuration.GetSection("MySetting"));

            services.AddRouting(options =>
            {
                // 将 URL 地址转换成小写  （设置后，swagger/html.index页面展现的api会全部是小写的）
                options.LowercaseUrls = true;
            });
            services.AddHostedService<TokenRefreshService>();

            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddMemoryCache();
            this.ConfigureServices_DistributedRedisCache(services);
            // this.ConfigureServices_DistributedSqlServerCache(services);

            this.ConfigureServices_HttpClient(services);

            this.lockFactory = this.GetRedLockFactory();
            services.AddSingleton(typeof(IDistributedLockFactory), this.lockFactory);

            return services.GetAutofacServiceProvider();
        }

        private RedLockFactory GetRedLockFactory()
        {
            var redisUrl = this.Configuration["RedisUrl"];
            if (string.IsNullOrEmpty(redisUrl))
            {
                throw new ArgumentException("RedisUrl 不能为空");
            }
            var urls = redisUrl.Split(";").ToList();
            var endPoints = new List<RedLockEndPoint>();
            foreach (var item in urls)
            {
                var arr = item.Split(":");
                var otherParams = arr[1].Split(",");
                endPoints.Add(new RedLockEndPoint(new DnsEndPoint(arr[0], Convert.ToInt32(otherParams[0]))) { Password = otherParams[1], RedisDatabase = 1 });
            }
            return RedLockFactory.Create(endPoints);
        }

        private void ConfigureServices_DistributedRedisCache(IServiceCollection services)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = "stu:";
                options.Configuration = this.Configuration["RedisConnectionString"];
            });
        }

        private void ConfigureServices_DistributedSqlServerCache(IServiceCollection services)
        {
            // 使用 options.SystemClock 配置了一个本地时钟, 缓存过期扫描时钟
            // 接着设置缓存过期时间为 1 分钟，缓存过期后逐出时间为 5 分钟
            services.AddDistributedSqlServerCache(options =>
            {
                options.SystemClock = new LocalSystemClock();
                options.ConnectionString = this.Configuration["ConnectionString"];
                options.SchemaName = "dbo";
                options.TableName = "AspNetCoreCache";
                options.DefaultSlidingExpiration = TimeSpan.FromMinutes(1);
                options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(5);
            });
        }

        private void ConfigureServices_HttpClient(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient("github", c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");
                // Github API versioning
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                // Github requires a user-agent
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });
            services.AddHttpClient<GitHubService>(c =>
            {
                // Github requires a user-agent
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });

            // 通道中处理出站请求 , 可以按处理程序应该执行的顺序注册多个处理程序。
            services.AddTransient<ValidateHeaderHandler>();
            services.AddHttpClient("externalservice", c =>
            {
                c.BaseAddress = new Uri("https://localhost:5000/");
            })
            .AddHttpMessageHandler<ValidateHeaderHandler>();

            // Polly
            services.AddHttpClient("PollyClient")
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            app.UseMiddleware<AdminSafeListMiddleware>(this.Configuration["AdminSafeList"]);
            app.UseErrorHandlingMiddleware();

            // nlog            
            loggerFactory.AddNLog();
            env.ConfigureNLog("Nlog.config");

            app.UseResponseCompression();
            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(10)
                };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };

                await next();
            });           

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();

                app.UseSwagger()
                    .UseSwaggerUI(options =>
                    {
                        options.DocumentTitle = "Leopard.Template.WebAPI Swagger 测试文档";
                        foreach (var item in docs)
                            options.SwaggerEndpoint($"/swagger/{item}/swagger.json", item);
                    });
            }

            lifetime.ApplicationStopping.Register(() =>
            {
                this.lockFactory.Dispose();
            });

            app.UseMvc();
        }
    }
}
