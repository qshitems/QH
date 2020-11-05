using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QH.Core.Output;
using QH.Core.Result;

namespace QH.Api.Attributes
{
    /// <summary>
    /// 输入模型验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ValidateInputAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                try
                {
                    context.Result = new JsonResult(ResultModel.Failed(context.ModelState.Values.First().Errors[0].ErrorMessage));
                }
                catch
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
