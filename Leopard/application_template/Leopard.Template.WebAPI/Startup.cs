using Leopard.AspNetCore.Middleware;
using Leopard.Configuration;
using Leopard.Template.WebAPI.Config;
using Leopard.AspNetCore.Autofac.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using Leopard.Template.WebAPI.HttpClients;
using Polly;
using System.Net.Http;

namespace Leopard.Template.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Env = env;
            this.Configuration = configuration;
            this.Configuration.AddConfigurationGeterLocator();
        }

        public IHostingEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        static string[] docs = new[] { "未分类", "Swagger组" };

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddIConfigurationGeter().AddConfigFinder();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

            services.Configure<MySetting>(this.Configuration.GetSection("MySetting"));

            services.AddRouting(options =>
            {
                // 将 URL 地址转换成小写  （设置后，swagger/html.index页面展现的api会全部是小写的）
                options.LowercaseUrls = true;
            });

            return services.GetAutofacServiceProvider();
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseErrorHandlingMiddleware();
            // nlog            
            loggerFactory.AddNLog();
            env.ConfigureNLog("Nlog.config");

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

            app.UseMvc();
        }
    }
}
