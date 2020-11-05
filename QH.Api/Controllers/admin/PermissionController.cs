using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Services;

namespace QH.Api.Controllers.admin
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class PermissionController : AreaController
    {
        private readonly IPermissionService _permissionServices;

        public PermissionController(IPermissionService permissionServices)
        {
            _permissionServices = permissionServices;
        }

        /// <summary>
        /// 查询权限列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetList(string key)
        {
            return await _permissionServices.ListAsync(key);
        }

        /// <summary>
        /// 查询单条分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetGroup(int id)
        {
            return await _permissionServices.GetGroupAsync(id);
        }

        /// <summary>
        /// 查询单条菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetMenu(int id)
        {
            return await _permissionServices.GetMenuAsync(id);
        }

        /// <summary>
        /// 查询单条接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetApi(int id)
        {
            return await _permissionServices.GetApiAsync(id);
        }

        /// <summary>
        /// 查询单条权限点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetDot(int id)
        {
            return await _permissionServices.GetDotAsync(id);
        }

        /// <summary>
        /// 查询角色权限-权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetPermissionList()
        {
            return await _permissionServices.GetPermissionList();
        }

        /// <summary>
        /// 查询角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetRolePermissionList(int roleId = 0)
        {
            return await _permissionServices.GetRolePermissionList(roleId);
        }

        /// <summary>
        /// 新增分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> AddGroup(PermissionAddGroupInput input)
        {
            return await _permissionServices.AddGroupAsync(input);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> AddMenu(PermissionAddMenuInput input)
        {
            return await _permissionServices.AddMenuAsync(input);
        }

        /// <summary>
        /// 新增接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> AddApi(PermissionAddApiInput input)
        {
            return await _permissionServices.AddApiAsync(input);
        }

        /// <summary>
        /// 新增权限点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> AddDot(PermissionAddDotInput input)
        {
            return await _permissionServices.AddDotAsync(input);
        }

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> UpdateGroup(PermissionUpdateGroupInput input)
        {
            return await _permissionServices.UpdateGroupAsync(input);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> UpdateMenu(PermissionUpdateMenuInput input)
        {
            return await _permissionServices.UpdateMenuAsync(input);
        }

        /// <summary>
        /// 修改接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> UpdateApi(PermissionUpdateApiInput input)
        {
            return await _permissionServices.UpdateApiAsync(input);
        }

        /// <summary>
        /// 修改权限点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> UpdateDot(PermissionUpdateDotInput input)
        {
            return await _permissionServices.UpdateDotAsync(input);
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultModel> SoftDelete(int id)
        {
            return await _permissionServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> Assign(PermissionAssignInput input)
        {
            return await _permissionServices.AssignAsync(input);
        }
    }
}
