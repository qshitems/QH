using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QH.Core.Attributes;
using QH.Core.Extensions;
using QH.Core.Result;
using QH.Services;

namespace Admin.Core.Auth
{
    /// <summary>
    /// 权限处理
    /// </summary>
    [SingleInstance]
    public class PermissionHandler : IPermissionHandler
    {
        private readonly IUserService _userService;

        public PermissionHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name = "api"> 接口路径 </param>
        /// <param name="httpMethod">http请求方法</param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(string api, string httpMethod)
        {
            var res = await _userService.GetPermissionsAsync();
            ResultModel<List<string>> output = null;
            if (res.Success)
            {
                output = res as ResultModel<List<string>>;
                var isValid = output.Data.Any(m => m.EqualsIgnoreCase($"{httpMethod}/{api}"));
                //  var isValid = permissions.Any(m => m != null && m.EqualsIgnoreCase($"/{api}"));
                return isValid;
            }
            return false;
          
        }
    }
}
