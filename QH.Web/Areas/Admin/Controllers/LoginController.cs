using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QH.Core.Auth;
using QH.Core.CodeGenerator;
using QH.Core.Extensions;
using QH.Core.Helpers;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Services;
using QH.Web.Attributes;
using QH.Web.HttpApis;

namespace QH.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class LoginController : BaseController
    {


        private readonly IAuthService _authService;
        private readonly IAuthApi _authApi;
        public LoginController(IAuthService authService, IAuthApi authApi) 
        {
            _authService = authService;
            _authApi = authApi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            var result = _authApi.Test(11);

            //  _CodeGenerator.GenerateTemplateCodesFromDatabase();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthLoginInput loginModel)
        {

            var sw = new Stopwatch();
            sw.Start();
            var res = await _authService.LoginAsync(loginModel);
            //  var  res = await _authApi.Login(loginModel);
            sw.Stop();
            #region

            var loginLogAddInput = new LoginLogAddInput()
            {
                CreatedUserName = loginModel.UserName,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Msg = res.Msg
            };

            ResultModel<AuthLoginOutput> output = null;
            if (res.Success)
            {
                output = res as ResultModel<AuthLoginOutput>;
                var _user = output.Data;
                loginLogAddInput.CreatedUserId = _user.Id;
                loginLogAddInput.NickName = _user.NickName;
            }

            // await _loginLogService.AddAsync(loginLogAddInput);
            #endregion

            if (!res.Success)
            {
                return Json(res);
            }
            var userData = output.Data;
            var claims = new[]
            {
                new Claim(ClaimAttributes.UserId, userData.Id.ToString()),
                new Claim(ClaimAttributes.UserName, userData.UserName),
                new Claim(ClaimAttributes.UserNickName, userData.NickName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            user, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                AllowRefresh = true
            });
            return Json(res);
        }


        //public async Task<IActionResult> Login([FromBody] AuthLoginInput loginModel)
        //{
        //    var res = await _authApi.Login(loginModel);
        //    if (!res.Success)
        //    {
        //        return Json(res);
        //    }
        //    var userData = res.Data;
        //    var claims = new[]
        //    {
        //        new Claim(ClaimAttributes.UserId, userData.Id.ToString()),
        //        new Claim(ClaimAttributes.UserName, userData.UserName),
        //        new Claim(ClaimAttributes.UserNickName, userData.NickName),
        //         new Claim("AccessToken", userData.AccessToken)

        //    };
        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
        //    await HttpContext.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    user, new AuthenticationProperties()
        //    {
        //        IsPersistent = true,
        //        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
        //        AllowRefresh = true
        //    });
        //    return Json(res);
        //}


    }
}