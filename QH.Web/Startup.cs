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
                options.DbType = DatabaseType.SqlServer.ToString();//���ݿ�������SqlServer,�����������Ͳ���ö��DatabaseType
                options.Author = "QSH";//��������
                options.OutputPath = "E:\\QHCodeGenerator";//ģ��������ɵ�·��
                options.ModelsNamespace = "QH.Models";//ʵ�������ռ�
                options.IRepositoryNamespace = "QH.IRepository";//�ִ��ӿ������ռ�
                options.RepositoryNamespace = "QH.Repository";//�ִ������ռ�
                options.IServicesNamespace = "QH.IServices";//����ӿ������ռ�
                options.ServicesNamespace = "QH.Services";//���������ռ�
            });
            //���ݿ�����
            services.Configure<DbOption>(Configuration.GetSection("DbOption"));
            services.AddSingleton(new Appsettings(Configuration));
            services.AddScoped<CodeGenerator>();
            //     services.AddControllers().AddControllersAsServices(); //����ע�����������  

            //DI��AutoMapper����Ҫ�õ��ķ������а���AutoMapper�������� Profile



            //#region ����mvc����
            ////ע��mvc����
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<LogExceptionFilter>();//����ȫ���쳣���������
            //});

            //#endregion


            //�û���Ϣ
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IUser, User>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
                {
                    option.AccessDeniedPath = "/Admin/login"; //���û����Է�����Դ��û��ͨ���κ���Ȩ����ʱ������������ض�������·����Դ
                    option.LoginPath = "/Admin/login";
                    //option.Cookie.Name = "token";//���ô洢�û���¼��Ϣ���û�Token��Ϣ����Cookie����
                    option.Cookie.HttpOnly = true;//���ô洢�û���¼��Ϣ���û�Token��Ϣ����Cookie���޷�ͨ���ͻ���������ű�(��JavaScript��)���ʵ�
                    option.Cookie.SecurePolicy = CookieSecurePolicy.Always;//���ô洢�û���¼��Ϣ���û�Token��Ϣ����Cookie��ֻ��ͨ��HTTPSЭ�鴫�ݣ������HTTPЭ�飬Cookie���ᱻ���͡�ע�⣬option.Cookie.SecurePolicy���Ե�Ĭ��ֵ��Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest
                });


            #region AutoMapper �Զ�ӳ��
            var serviceAssembly = Assembly.Load("QH.Models");
            services.AddAutoMapper(serviceAssembly);
            #endregion

            #region ����
            services.AddMemoryCache();
            services.AddSingleton<ICache, MemoryCache>();

            #endregion

            #region ����Session����
            //����Session��������redis���Զ���ʼ�ֲ�ʽsession
            services.AddSession();
            #endregion

            //���HttpClient���
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

            //�������԰��ļ�������
            //services.AddLocalization(o =>
            //{
            //    o.ResourcesPath = "Language";
            //});
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {

            #region SingleInstance
            //�޽ӿ�ע�뵥��
            var assemblyCore = Assembly.Load("QH.Web");
            var assemblyCommon = Assembly.Load("QH.Core");
            builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .SingleInstance();
            //�нӿ�ע�뵥��
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


            //��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();
            //Session�м��
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //����·������
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
            //    //����ָ��Ĭ�����԰�
            //    DefaultRequestCulture = new RequestCulture("zh-CN"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //});
        }
    }
}
