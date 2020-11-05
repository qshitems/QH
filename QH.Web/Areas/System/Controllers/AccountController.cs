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
using QH.Core.Output;
using QH.Models;
using QH.Services;

namespace QH.Web.Areas.System.Controllers
{
    [Area("system")]
    public class AccountController : BaseController
    {
        protected readonly ILogger<AccountController> _logger;
        private readonly IAuthService _authService;
        public AccountController(ILogger<AccountController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            ViewBag.SoftwareName = "QH";
            return View();
        }

        /// <summary>
        /// 获取验证码图片。
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult VerifyCode()
        //{
        //    //VerifyCode verify = new VerifyCode();
        //    //HttpContext.Session.SetString(Keys.SESSION_KEY_VCODE, verify.Text.ToLower());
        //    return File(verify.Image, "image/jpeg");
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName, string passWord)
        {

            var sw = new Stopwatch();
            sw.Start();
            var res = await _authService.LoginAsync(new AuthLoginInput { UserName = userName, Password = passWord });
            sw.Stop();
            #region

            var loginLogAddInput = new LoginLogAddInput()
            {
                CreatedUserName = userName,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Msg = res.Msg
            };
            ResponseOutput<AuthLoginOutput> output = null;
            if (res.Success)
            {
                output = res as ResponseOutput<AuthLoginOutput>;
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
    }
}
