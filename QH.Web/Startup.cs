using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using QH.Core.Attributes;
using QH.Core.Auth;
using QH.Core.Cache;
using QH.Core.CodeGenerator;
using QH.Core.Helpers;
using QH.Core.Models;
using QH.Core.Options;
using WebApiClient;

namespace QH.Web
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
            services.AddControllersWithViews();

            services.Configure<CodeGenerateOption>(options =>
            {
                options.ConnectionString = Configuration.GetValue<string>("DbOption:ConnectionString");
                // options.ConnectionString = "Data Source=.;Initial Catalog=HKSJPersonManage;User ID=sa;Password=000000;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                options.DbType = DatabaseType.SqlServer.ToString();//数据库类型是SqlServer,其他数据类型参照枚举DatabaseType
                options.Author = "QSH";//作者名称
                options.OutputPath = "E:\\QHCodeGenerator";//模板代码生成的路径
                options.ModelsNamespace = "QH.Models";//实体命名空间
                options.IRepositoryNamespace = "QH.IRepository";//仓储接口命名空间
                options.RepositoryNamespace = "QH.Repository";//仓储命名空间
                options.IServicesNamespace = "QH.IServices";//服务接口命名空间
                options.ServicesNamespace = "QH.Services";//服务命名空间
            });
            //数据库连接
            services.Configure<DbOption>(Configuration.GetSection("DbOption"));
            services.AddSingleton(new Appsettings(Configuration));
            services.AddScoped<CodeGenerator>();
            //     services.AddControllers().AddControllersAsServices(); //属性注入必须加上这个  

            //DI了AutoMapper中需要用到的服务，其中包括AutoMapper的配置类 Profile



            //#region 启用mvc服务
            ////注册mvc服务
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<LogExceptionFilter>();//设置全局异常处理过滤器
            //});

            //#endregion


            //用户信息
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IUser, User>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
                {
                    option.AccessDeniedPath = "/Admin/login"; //当用户尝试访问资源但没有通过任何授权策略时，这是请求会重定向的相对路径资源
                    option.LoginPath = "/Admin/login";
                    //option.Cookie.Name = "token";//设置存储用户登录信息（用户Token信息）的Cookie名称
                    option.Cookie.HttpOnly = true;//设置存储用户登录信息（用户Token信息）的Cookie，无法通过客户端浏览器脚本(如JavaScript等)访问到
                    option.Cookie.SecurePolicy = CookieSecurePolicy.Always;//设置存储用户登录信息（用户Token信息）的Cookie，只会通过HTTPS协议传递，如果是HTTP协议，Cookie不会被发送。注意，option.Cookie.SecurePolicy属性的默认值是Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest
                });


            #region AutoMapper 自动映射
            var serviceAssembly = Assembly.Load("QH.Models");
            services.AddAutoMapper(serviceAssembly);
            #endregion

            #region 缓存
            services.AddMemoryCache();
            services.AddSingleton<ICache, MemoryCache>();

            #endregion

            #region 启用Session服务
            //启用Session服务，启用redis，自动开始分布式session
            services.AddSession();
            #endregion

            //添加HttpClient相关
            var types = typeof(Startup).Assembly.GetTypes()
                        .Where(type => type.IsInterface
                        && ((System.Reflection.TypeInfo)type).ImplementedInterfaces != null
                        && type.GetInterfaces().Any(a => a.FullName == typeof(IHttpApi).FullName));
            foreach (var type in types)
            {
                services.AddHttpApi(type);
                services.ConfigureHttpApi(type, o =>
                {
                    o.HttpHost = new Uri(Appsettings.App("Setting:ApiUrl"));
                });
            }

            //设置语言包文件夹名称
            //services.AddLocalization(o =>
            //{
            //    o.ResourcesPath = "Language";
            //});
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {

            #region SingleInstance
            //无接口注入单例
            var assemblyCore = Assembly.Load("QH.Web");
            var assemblyCommon = Assembly.Load("QH.Core");
            builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .SingleInstance();
            //有接口注入单例
            builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .AsImplementedInterfaces()
            .SingleInstance();
            #endregion
            #region Repository
            var assemblyRepository = Assembly.Load("QH.Repository");
            builder.RegisterAssemblyTypes(assemblyRepository)
            .AsImplementedInterfaces()
            .InstancePerDependency();
            #endregion

            #region Service
            var assemblyServices = Assembly.Load("QH.Services");
            builder.RegisterAssemblyTypes(assemblyServices)
            .AsImplementedInterfaces()
            .InstancePerDependency();
            #endregion
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();
            //Session中间件
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //区域路由配置
                //endpoints.MapAreaControllerRoute(
                //     name: "areas", "areas",
                //   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "areaadmin",
                    areaName: "admin",
                    pattern: "admin/{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapAreaControllerRoute(
                //    name: "areasystem",
                //    areaName: "system",
                //    pattern: "system/{controller=Home}/{action=Index}/{id?}");
            });

            //IList<CultureInfo> supportedCultures = new List<CultureInfo>
            //{
            //    new CultureInfo("zh-CN"),
            //    new CultureInfo("zh"),
            //    new CultureInfo("en-US")
            //};
            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    //这里指定默认语言包
            //    DefaultRequestCulture = new RequestCulture("zh-CN"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //});
        }
    }
}
