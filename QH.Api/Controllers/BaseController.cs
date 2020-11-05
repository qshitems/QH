using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QH.Api.Attributes;
using QH.Apie.Attributes;

namespace QH.Api.Controllers
{
   // [Route("api/[controller]")]
  
    /// <summary>
    /// 基础控制器
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
   // [Permission]
    [ValidateInput]
    public class BaseController : ControllerBase
    {
    }
}
