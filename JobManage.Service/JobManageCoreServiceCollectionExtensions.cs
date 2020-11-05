using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QH.Core.Auth;
using QH.Core.Helpers;
using QH.Core.Options;
using QH.IRepository;
using QH.Repository;
using QH.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JobManage.Service
{
  public static  class JobManageCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {

            //用户信息
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IUser, User>();
            services.AddTransient<IJobRepository, JobRepository>();
            services.AddTransient<IJobRunLogRepository, JobRunLogRepository>();
            services.AddTransient<IJobService,QH.Services.JobService>();
            services.AddTransient<IJobRunLogService, JobRunLogService>();

            return services;
        }
    }
}
