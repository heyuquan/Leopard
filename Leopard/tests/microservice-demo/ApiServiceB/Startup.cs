using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Leopard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiServiceB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            String ip = "localhost";
            int port = 50440;
            var client = new ConsulClient(ConfigurationOverview);       //回调获取
            string serviceId = "ServerNameFirst" + Guid.NewGuid();

            var result = client.Agent.ServiceRegister(
                new AgentServiceRegistration()
                {
                    ID = serviceId,          //服务编号保证不重复
                    Name = "ApiServiceB",    //服务的名称
                    Address = ip,            //服务ip地址
                    Port = port,             //服务端口
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = CacheTimeSpan.DefaultCacheTime,    //服务启动多久后反注册
                        Interval = CacheTimeSpan.DefaultCacheTime,      //健康检查时间间隔，或者称为心跳间隔（定时检查服务是否健康）
                        HTTP = $"http://{ip}:{port}/api/health",         //健康检查地址
                        Timeout = CacheTimeSpan.DefaultCacheTime
                    }
                });

            lifetime.ApplicationStopping.Register(
                () =>
                {
                    client.Agent.ServiceDeregister(serviceId).Wait();
                });
        }

        private static void ConfigurationOverview(ConsulClientConfiguration obj)
        {
            //consul的地址
            obj.Address = new Uri("http://127.0.0.1:8500");
            //数据中心命名
            obj.Datacenter = "dc1";
        }
    }
}
