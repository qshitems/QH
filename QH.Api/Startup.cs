using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Admin.Core.Auth;
using Autofac;
using AutoMapper;
using Dapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Extensions.Logging;
using QH.Api.Filters;
using QH.Core.Attributes;
using QH.Core.Auth;
using QH.Core.Cache;
using QH.Core.Configs;
using QH.Core.Helpers;
using QH.Core.Options;
using QH.Services;

namespace QH.Api
{
    public class Startup
    {
        private static string basePath => AppContext.BaseDirectory;
        private readonly IHostEnvironment _env;
        private readonly ConfigHelper _configHelper;
        private readonly AppConfig _appConfig;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _configHelper = new ConfigHelper();
            _appConfig = _configHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();


        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton(new Appsettings(Configuration));

            //用户信息
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IUser, User>();
            //数据库连接
            services.Configure<DbOption>(Configuration.GetSection("DbOption"));

            //应用配置
            services.AddSingleton(_appConfig);

            //上传配置
            var uploadConfig = _configHelper.Load("uploadconfig", _env.EnvironmentName, true);
            services.Configure<UploadConfig>(uploadConfig);


            #region AutoMapper 自动映射
            var serviceAssembly = Assembly.Load("QH.Models");
            services.AddAutoMapper(serviceAssembly);
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    string version = "V1";
                    //typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                    //{
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = "QH.Api"
                    });
                    //c.OrderActionsBy(o => o.RelativePath);
                    // });

                    var xmlPath = Path.Combine(basePath, "QH.Api.xml");
                    c.IncludeXmlComments(xmlPath, true);

                    //var xmlCommonPath = Path.Combine(basePath, "Admin.Core.Common.xml");
                    //c.IncludeXmlComments(xmlCommonPath, true);

                    //var xmlModelPath = Path.Combine(basePath, "Admin.Core.Model.xml");
                    //c.IncludeXmlComments(xmlModelPath);

                    //var xmlServicesPath = Path.Combine(basePath, "Admin.Core.Service.xml");
                    // c.IncludeXmlComments(xmlServicesPath);

                    //添加设置Token的按钮
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = "Authorization",//jwt默认的参数名称
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    //添加Jwt验证设置
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
                });
            }
            #endregion

            #region Jwt身份认证
            var jwtConfig = _configHelper.Get<JwtConfig>("jwtconfig", _env.EnvironmentName);
            services.TryAddSingleton(jwtConfig);
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { }); ;
            #endregion

            #region 操作日志
            if (_appConfig.Log.Operation)
            {
                // services.AddSingleton<ILogHandler, LogHandler>();
            }
            #endregion

            #region 控制器
            services.AddControllers(options =>
            {
                options.Filters.Add<AdminExceptionFilter>();
                //if (_appConfig.Log.Operation)
                //{
                //    options.Filters.Add<LogActionFilter>();
                //}
                //禁止去除ActionAsync后缀
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //使用驼峰 首字母小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion

            #region 缓存
            var cacheConfig = _configHelper.Get<CacheConfig>("cacheconfig", _env.EnvironmentName);
            if (cacheConfig.Type == CacheType.Redis)
            {
                //var csredis = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionString);
                //RedisHelper.Initialization(csredis);
                //services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemoryCache>();
            }
            #endregion
            //  services.AddScoped<IUserToken, UserToken>();

            //阻止NLog接收状态消息
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

         services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetValue<string>("DbOption:ConnectionString")));

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            #region SingleInstance
            //无接口注入单例
            var assemblyCore = Assembly.Load("QH.Api");
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
            app.UseHttpsRedirection();

            //静态文件
            // app.UseUploadConfig();
            //路由
            app.UseRouting();

            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();
            //异常
            app.UseExceptionHandler("/Error");
            //配置端点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region Swagger Api文档
            if (_env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    string version = "V1";
                    //typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    //{
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"QH.Api {version}");
                    // });
                    c.RoutePrefix = "";//直接根目录访问，如果是IIS发布可以注释该语句，并打开launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api
                    c.DefaultModelsExpandDepth(-1);//不显示Models
                });
            }
            #endregion

            #region nlog日志入库配置
            //  LogManager.Configuration.Variables["connectionString"] = Configuration.GetValue<string>("DbOption:ConnectionString");
            #endregion

            //app.UseHangfireServer();//启动hangfire 服务
            //var options = new DashboardOptions()

            //{
            //    Authorization = new[] { new HangfireAuthorizationFilter() },

            //};
            //app.UseHangfireDashboard("/jobs", options);//启动hangfire 面板

  
  

            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest(), CronHelper.Minutely());
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest1(), CronHelper.MinuteInterval(3));
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest2(), CronHelper.Hourly());
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest3(), CronHelper.HourInterval(3));
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest4(), CronHelper.SecondInterval(30));

        }
    }
}
