﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;
using QH.Core.Auth;

namespace QH.Web.Attributes
{
    /// <summary>
    /// 启用权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        public  void OnAuthorization(AuthorizationFilterContext context)
        {
            //排除匿名访问
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)))
                return;
            //登录验证
            var user = context.HttpContext.RequestServices.GetService<IUser>();
            if (user == null || !(user?.Id > 0))
            {
                context.Result = new ChallengeResult();
                return;
            }

            ////权限验证
            //var httpMethod = context.HttpContext.Request.Method;
            //var api = context.ActionDescriptor.AttributeRouteInfo.Template;
            //var permissionHandler = context.HttpContext.RequestServices.GetService<IPermissionHandler>();
            //var isValid = await permissionHandler.ValidateAsync(api, httpMethod);
            //if (!isValid)
            //{
            //    context.Result = new ForbidResult();
            //}
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
             OnAuthorization(context);
            return Task.CompletedTask;
        }
    }
}