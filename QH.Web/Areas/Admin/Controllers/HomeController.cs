using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QH.Core.Auth;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Models.ViewModel;
using QH.Services;

namespace QH.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeController : BaseController
    {
        protected readonly ILogger<HomeController> _logger;
        private readonly IPermissionService _permissionService;
        public HomeController(ILogger<HomeController> logger, IPermissionService permissionService, IUser user) 
        {
            _logger = logger;
            _user = user;
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = _user.Name;
            return View();
        }

        public IActionResult Default()
        {
            return View();
        }

        public async Task<IActionResult> Menus()
        {
            var result = await _permissionService.GetMenuList(_user.Id);
            ResultModel<List<MenuModel>> res = null;
            if (result.Success)
                res = result as ResultModel<List<MenuModel>>;
            return Json(res.Data);
        }

    }
}
