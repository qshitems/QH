using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QH.Core.Auth;

namespace QH.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUser _user;
        protected ILogger<HomeController> _logger;


    }
}
