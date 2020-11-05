using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QH.Core.Extensions;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Services;

namespace QH.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Form(int? id)
        {
            PermissionEntity entity = new PermissionEntity();
            if (id > 0)
            {
                var result = await _permissionService.GetAsync(id.Value) as ResultModel<PermissionEntity>;
                if (result.Success)
                    entity = result.Data;
            }
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(PermissionPageInput input)
        {
            PageOutput<PermissionPageOutput> output = new PageOutput<PermissionPageOutput>();
            var result = await _permissionService.PageAsync(input) as ResultModel<PageOutput<PermissionPageOutput>>;
            if (result.Success)
                output = result.Data;
            return Json(output);
        }

        //[HttpPost]
        //public async Task<IActionResult> SubmitForm(RoleEntity input)
        //{
        //    var result = await _permissionService.AddOrUpdateAsync(input);
        //    return Json(result);
        //}

        //[HttpPut]
        //public async Task<IActionResult> Enable(long id)
        //{
        //    var result = await _permissionService.IsEnable(id, true);
        //    return Json(result);
        //}
        //[HttpPut]
        //public async Task<IActionResult> Disable(long id)
        //{
        //    var result = await _permissionService.IsEnable(id, false);
        //    return Json(result);
        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _permissionService.DeleteAsync(id);
            return Json(result);
        }


        [HttpDelete]
        public async Task<IActionResult> BatchRemove( string id)
        {
          
            var result = await _permissionService.DeleteListAsync(id);
            return Json(result);
        }

        
    }
}
