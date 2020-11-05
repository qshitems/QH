using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QH.Core;
using QH.Core.Input;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Services;

namespace QH.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class RoleController : BaseController
    {

        private readonly IRoleService _roleServices;
        private readonly IPermissionService _permissionService;

        public RoleController(IRoleService roleServices,IPermissionService permissionService)
        {
            _roleServices = roleServices;
            _permissionService = permissionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Form(int? id)
        {
            RoleEntity entity = new RoleEntity();
            if (id > 0)
            {
                var result = await _roleServices.GetAsync(id.Value) as ResultModel<RoleEntity>;
                entity = result.Data;
            }
            return View(entity);
        }
        [HttpGet]
        public async Task<IActionResult> GetPage(RoleListInput input)
        {
            var result = await _roleServices.PageAsync(input) as ResultModel<PageOutput<RoleListOutput>>;
            return Json(result.Data);
        }

        public IActionResult Power(long id)
        {
            ViewBag.id = id;
            return View();
        }

        public async Task<IActionResult> GetRolePower(int id)
        {
            var result = await _permissionService.GetPermissionListTree(id) as ResultModel<DTreeModel>;
            DTreeModel dTree = result.Data;
            return Json(dTree);
        }

        [HttpGet]
        public async Task<IActionResult> GetForm(int id)
        {
            var result = await _roleServices.GetAsync(id) as ResultModel<RoleEntity>;
            return Json(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> SetRolePower(PermissionAssignInput input)
        {
            var result = await _permissionService.AssignAsync(input) ;
            return Json(result);
        }

   
        [HttpPost]
        public async Task<IActionResult> SubmitForm(RoleEntity input)
        {
            var result = await _roleServices.AddOrUpdateAsync(input);
            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> Enable(int id)
        {
            var result = await _roleServices.IsEnable(id, true);
            return Json(result);
        }
        [HttpPut]
        public async Task<IActionResult> Disable(int id)
        {
            var result = await _roleServices.IsEnable(id, false);
            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _roleServices.SoftDeleteAsync(id);
            return Json(result);
        }
    }
}
