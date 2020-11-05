﻿using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Logs;
using Microsoft.AspNetCore.Mvc.Filters;
using QH.Api.Attributes;

namespace QH.Api.Filters
{
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogHandler _logHandler;

        public LogActionFilter(ILogHandler logHandler)
        {
            _logHandler = logHandler;
        }
        public LogActionFilter()
        {
           
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(NoOprationLogAttribute)))
            {
                return next();
            }

            return _logHandler.LogAsync(context, next);
        }
    }
}
