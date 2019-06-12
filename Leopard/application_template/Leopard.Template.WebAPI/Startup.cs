using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Leopard.AspNetCore.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NLog.Extensions.Logging;
using NLog.Web;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Leopard.Template.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Env = env;
            this.Configuration = configuration;
        }

        public IHostingEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        static string[] docs = new[] { "未分类", "Swagger组" };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddRouting(options =>
            {
                // 将 URL 地址转换成小写  （设置后，swagger/html.index页面展现的api会全部是小写的）
                options.LowercaseUrls = true;
            });
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
