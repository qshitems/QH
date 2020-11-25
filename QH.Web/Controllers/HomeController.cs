using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using QH.Core.Auth;
using QH.Core.CodeGenerator;
using QH.Web.Models;

namespace QH.Web.Controllers
{
    public class HomeController : Controller
    {



         private readonly CodeGenerator _CodeGenerator;
        //private readonly ITappUserService _userService;
        //public HomeController(ILogger<HomeController> logger, CodeGenerator codeGenerator, ITappUserService userService)
        //{
        //    _logger = logger;
        //    _CodeGenerator = codeGenerator;
        //    _userService = userService;
        //}

        public HomeController(CodeGenerator codeGenerator)
        {
            _CodeGenerator = codeGenerator;
            // _logger.LogDebug(1, "NLog injected into HomeController");


            // await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
        }


        public IActionResult Index()
        {
           // _CodeGenerator.GenerateTemplateCodesFromDatabase();
            // _userService.test();
    


            return View();
        }

    
    

    public IActionResult Privacy()
        {
            return View();
        }



     
    }
}
