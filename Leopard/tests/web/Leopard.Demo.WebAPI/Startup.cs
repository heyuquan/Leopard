using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

using log4net;
using log4net.Config;
using log4net.Repository;

using NLog.Extensions.Logging;
using NLog.Web;

namespace Leopard.Demo.WebAPI
{
    public class Startup
    {
        public static ILoggerRepository repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // log4net
            repository = LogManager.CreateRepository("FileErrorLogger");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));


        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // 默认加载 log4net.config 配置文件的 root 节点
            // loggerFactory.AddLog4Net();

            // nlog
            loggerFactory.AddNLog();
            env.ConfigureNLog("Nlog.config");
            
            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
