using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QH.Core.Auth;
using QH.Web.Attributes;
namespace QH.Web.Areas
{
    //[Route("[area]/[controller]/[action]")]
    [Permission]
    public class BaseController : Controller
    {
        protected IUser _user;


        //public BaseController(IUser user)
        //{
        //    _user = user;
        // }


    }

}
