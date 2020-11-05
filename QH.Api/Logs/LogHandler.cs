using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using QH.Core.Output;
using QH.Services;
using QH.Models;
using QH.Core.Result;
//using Newtonsoft.Json;

namespace Admin.Core.Logs
{
    /// <summary>
    /// 操作日志处理
    /// </summary>
    public class LogHandler : ILogHandler
    {
        private readonly IOprationLogService _oprationLogService;

        public LogHandler(
            IOprationLogService oprationLogService
        )
        {
            _oprationLogService = oprationLogService;
        }

        public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();
            dynamic actionResult = (await next()).Result;
            sw.Stop();

            //操作参数
            //var args = JsonConvert.SerializeObject(context.ActionArguments);
            //操作结果
            //var result = JsonConvert.SerializeObject(actionResult?.Value);

            var res = actionResult?.Value as IResultModel;

            var input = new OprationLogAddInput
            {
                ApiMethod = context.HttpContext.Request.Method.ToLower(),
                ApiPath = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res?.Success,
                Msg = res?.Msg
            };
            //  input.ApiLabel = _apiHelper.GetApis().FirstOrDefault(a => a.Path == input.ApiPath)?.Label;
         
              await _oprationLogService.AddAsync(input);
        }
    }
}
