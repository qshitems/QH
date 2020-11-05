using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QH.Core.Enums;
using QH.Core.Extensions;
using QH.Core.Output;
using QH.Core.Result;
using System.Threading.Tasks;

namespace QH.Api.Filters
{
    /// <summary>
    /// 异常错误过滤
    /// </summary>
    public class AdminExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AdminExceptionFilter> _logger;

        public AdminExceptionFilter(IWebHostEnvironment env, ILogger<AdminExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string message;
            if (_env.IsProduction())
            {
                message =StatusCodes.Status500InternalServerError.ToDescription();
            }
            else
            {
                message = context.Exception.Message;
            }

            _logger.LogError(context.Exception,"");
            var data = ResultModel.Failed(message);
            context.Result = new InternalServerErrorResult(data);
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object value) : base(value)
        {
            StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
        }
    }

}
