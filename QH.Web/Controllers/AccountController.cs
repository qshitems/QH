using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QH.Core.Auth;
using QH.Core.Output;
using QH.Core.Result;

namespace QH.Web.Controllers
{
    public class AccountController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
  
        public async Task<IActionResult> LoginOn(string name, string password, string checkcode)
        {
            var claims = new[]
           {
                new Claim(ClaimAttributes.UserId, "1"),
                new Claim(ClaimAttributes.UserName, "admin"),
                new Claim(ClaimAttributes.UserNickName, "admin")
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);


            //登录用户，相当于ASP.NET中的FormsAuthentication.SetAuthCookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

            //可以使用HttpContext.SignInAsync方法的重载来定义持久化cookie存储用户认证信息，例如下面的代码就定义了用户登录后60分钟内cookie都会保留在客户端计算机硬盘上，
            //即便用户关闭了浏览器，60分钟内再次访问站点仍然是处于登录状态，除非调用Logout方法注销登录。
            //注意其中的AllowRefresh属性，如果AllowRefresh为true，表示如果用户登录后在超过50%的ExpiresUtc时间间隔内又访问了站点，就延长用户的登录时间（其实就是延长cookie在客户端计算机硬盘上的保留时间），
            //例如本例中我们下面设置了ExpiresUtc属性为60分钟后，那么当用户登录后在大于30分钟且小于60分钟内访问了站点，那么就将用户登录状态再延长到当前时间后的60分钟。但是用户在登录后的30分钟内访问站点是不会延长登录时间的，
            //因为ASP.NET Core有个硬性要求，是用户在超过50%的ExpiresUtc时间间隔内又访问了站点，才延长用户的登录时间。
            //如果AllowRefresh为false，表示用户登录后60分钟内不管有没有访问站点，只要60分钟到了，立马就处于非登录状态（不延长cookie在客户端计算机硬盘上的保留时间，60分钟到了客户端计算机就自动删除cookie）

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            user, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                AllowRefresh = true
            });


            //  如果HttpContext.User.Identity.IsAuthenticated为true，

            //if (HttpContext.User.Identity.IsAuthenticated)   //HttpContext.User.Identities.Count()>0)
            //{
            //   await  HttpContext.AuthenticateAsync();
            //    //这里通过 HttpContext.User.Claims 可以将我们在Login这个Action中存储到cookie中的所有
            //    //claims键值对都读出来，比如我们刚才定义的UserName的值Wangdacui就在这里读取出来了
            //    var userName = HttpContext.User.Claims.First().Value;
            //    if (!string.IsNullOrEmpty(userName))
            //    {
            //        bool IsLogin = true;
            //    }
            //}
            //string str = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;
         
            return Json(ResultModel.Success());

        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Account/Login");
        }

    }
}
