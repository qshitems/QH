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

            //�û���Ϣ
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IUser, User>();
            //���ݿ�����
            services.Configure<DbOption>(Configuration.GetSection("DbOption"));

            //Ӧ������
            services.AddSingleton(_appConfig);

            //�ϴ�����
            var uploadConfig = _configHelper.Load("uploadconfig", _env.EnvironmentName, true);
            services.Configure<UploadConfig>(uploadConfig);


            #region AutoMapper �Զ�ӳ��
            var serviceAssembly = Assembly.Load("QH.Models");
            services.AddAutoMapper(serviceAssembly);
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Swagger Api�ĵ�
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

                    //�������Token�İ�ť
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                        Name = "Authorization",//jwtĬ�ϵĲ�������
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    //���Jwt��֤����
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

            #region Jwt�����֤
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

            #region ������־
            if (_appConfig.Log.Operation)
            {
                // services.AddSingleton<ILogHandler, LogHandler>();
            }
            #endregion

            #region ������
            services.AddControllers(options =>
            {
                options.Filters.Add<AdminExceptionFilter>();
                //if (_appConfig.Log.Operation)
                //{
                //    options.Filters.Add<LogActionFilter>();
                //}
                //��ֹȥ��ActionAsync��׺
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //ʹ���շ� ����ĸСд
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion

            #region ����
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

            //��ֹNLog����״̬��Ϣ
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

         services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetValue<string>("DbOption:ConnectionString")));

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            #region SingleInstance
            //�޽ӿ�ע�뵥��
            var assemblyCore = Assembly.Load("QH.Api");
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
            app.UseHttpsRedirection();

            //��̬�ļ�
            // app.UseUploadConfig();
            //·��
            app.UseRouting();

            //��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();
            //�쳣
            app.UseExceptionHandler("/Error");
            //���ö˵�
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #region Swagger Api�ĵ�
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
                    c.RoutePrefix = "";//ֱ�Ӹ�Ŀ¼���ʣ������IIS��������ע�͸���䣬����launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//�۵�Api
                    c.DefaultModelsExpandDepth(-1);//����ʾModels
                });
            }
            #endregion

            #region nlog��־�������
            //  LogManager.Configuration.Variables["connectionString"] = Configuration.GetValue<string>("DbOption:ConnectionString");
            #endregion

            //app.UseHangfireServer();//����hangfire ����
            //var options = new DashboardOptions()

            //{
            //    Authorization = new[] { new HangfireAuthorizationFilter() },

            //};
            //app.UseHangfireDashboard("/jobs", options);//����hangfire ���

  
  

            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest(), CronHelper.Minutely());
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest1(), CronHelper.MinuteInterval(3));
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest2(), CronHelper.Hourly());
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest3(), CronHelper.HourInterval(3));
            //RecurringJob.AddOrUpdate<IHangfireService>(h => h.HangfireTest4(), CronHelper.SecondInterval(30));

        }
    }
}
