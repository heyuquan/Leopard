using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leopard.Template.MVC
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 设置 IDistributedCache 后，AddSession也会存储在 redis 里面
            this.ConfigureServices_DistributedRedisCache(services);
            
            // 方式一：禁用Cookie同意功能
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // 禁用Cookie同意功能
            //    options.CheckConsentNeeded = context => false;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;

            //});
            //services.AddSession();

            // 方式二：标记会话Cookie是必要的
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRouting(options =>
            {
                // 将 URL 地址转换成小写  （设置后，swagger/html.index页面展现的api会全部是小写的）
                options.LowercaseUrls = true;
            });

            services.AddHttpContextAccessor();

            // Cookie 封装组件，支持加密
            services.AddCookieManager(options=> {
                options.AllowEncryption = false;
            });
        }

        private void ConfigureServices_DistributedRedisCache(IServiceCollection services)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = "mvc:";
                options.Configuration = this.Configuration["RedisConnectionString"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
