using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using QH.Api.Attributes;
using QH.Core.Auth;
using QH.Core.Extensions;
using QH.Core.Helpers;
using QH.Core.Result;
using QH.Models;
using QH.Services;

namespace QH.Api.Controllers.admin
{
    /// <summary>
    /// 授权管理
    /// </summary>
    public class AuthController : AreaController
    {
        private readonly IUserToken _userToken;
        private readonly IAuthService _authService;
        private readonly ILoginLogService _loginLogService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(
            IUserToken userToken,
            IAuthService authServices,
            ILogger<AuthController> logger,
            ILoginLogService loginLogService
        )
        {
            _userToken = userToken;
            _authService = authServices;
            _logger = logger;
            _loginLogService = loginLogService;
        }




        /// <summary>
        /// 获得token
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private IResultModel GetToken(ResultModel<AuthLoginOutput> output)
        {
            if (!output.Success)
            {
                return ResultModel.Failed(output.Msg);
            }

            var user = output.Data;
            output.Data.AccessToken = _userToken.Create(new[]
            {
                new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                new Claim(ClaimAttributes.UserName, user.UserName),
                new Claim(ClaimAttributes.UserNickName, user.NickName)
            });
            return output;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public string GetTest(string input)
        {

            return input;
        }



        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="lastKey">上次验证码键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public async Task<IResultModel> GetVerifyCode(string lastKey)
        {
            return await _authService.GetVerifyCodeAsync(lastKey);
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public async Task<IResultModel> GetPassWordEncryptKey()
        {
            return await _authService.GetPassWordEncryptKeyAsync();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Login]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IResultModel> GetUserInfo()
        {
            return await _authService.GetUserInfoAsync();
        }

        /// <summary>
        /// 用户登录
        /// 根据登录信息生成Token
        /// </summary>
        /// <param name="input">登录信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [NoOprationLog]
        [Description("用户名登录")]
        //[FromQuery] 
        public async Task<IResultModel> Login(AuthLoginInput input)
        {
            var sw = new Stopwatch();
            sw.Start();
            var res = await _authService.LoginAsync(input);
            // res=await IAuthApi.lo
            sw.Stop();

            #region 添加登录日志
            var loginLogAddInput = new LoginLogAddInput()
            {
                CreatedUserName = input.UserName,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Msg = res.Msg
            };

            ResultModel<AuthLoginOutput> output = null;
            if (res.Success)
            {
                output = (res as ResultModel<AuthLoginOutput>);
                var user = output.Data;
                loginLogAddInput.CreatedUserId = user.Id;
                loginLogAddInput.NickName = user.NickName;
            }
            await _loginLogService.AddAsync(loginLogAddInput);
            #endregion

            if (!res.Success)
            {
                return res;
            }
            return GetToken(output);
        }





        /// <summary>
        /// 刷新Token
        /// 以旧换新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IResultModel> Refresh([BindRequired] string token)
        //{
        //    var userClaims = _userToken.Decode(token);
        //    if (userClaims == null || userClaims.Length == 0)
        //    {
        //        return ResponseOutput.NotOk();
        //    }

        //    var refreshExpiresValue = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
        //    if (refreshExpiresValue.IsNull())
        //    {
        //        return ResponseOutput.NotOk();
        //    }

        //    var refreshExpires = refreshExpiresValue.ToDate();
        //    if (refreshExpires <= DateTime.Now)
        //    {
        //        return ResponseOutput.NotOk("登录信息已过期");
        //    }

        //    var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
        //    if (userId.IsNull())
        //    {
        //        return ResponseOutput.NotOk();
        //    }
        //    var output = await _userServices.GetLoginUserAsync(userId.ToLong());

        //    return GetToken(output);
        //}
    }
}
