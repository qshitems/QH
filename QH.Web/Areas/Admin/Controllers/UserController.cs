using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QH.Core.Auth;
using QH.Core.Input;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Services;

namespace QH.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class UserController : BaseController
    {

        protected readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
       // protected readonly IUser _user;
        public UserController(ILogger<UserController> logger, IUser user, IUserService userService) 
        {
            _logger = logger;
            _userService = userService;
            _user = user;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> List(PageInput<UserListInput> input)
        {
            var result = await _userService.PageAsync(input);
            ResultModel<PageOutput<UserListOutput>> res = null;
            if (result.Success)
                res = result as ResultModel<PageOutput<UserListOutput>>;
            return Json(res.Data);
        }

        public IActionResult CreateOrUpdate()
        {
            return View();
        }
        public IActionResult AddOrEdit(long? id)
        {
            return View();
        }
    }
}
