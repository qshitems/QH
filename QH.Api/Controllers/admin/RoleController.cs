using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QH.Core.Input;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Services;

namespace QH.Api.Controllers.admin
{

    public class RoleController : AreaController
    {
        private readonly IRoleService _roleServices;

        public RoleController(IRoleService roleServices)
        {
            _roleServices = roleServices;
        }

        /// <summary>
        /// 查询单条角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> Get(int id)
        {
            return await _roleServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> Page([FromQuery] RoleListInput input)
        {
            return await _roleServices.PageAsync(input);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> Add([FromQuery] RoleAddInput input)
        {
            return await _roleServices.AddAsync(input);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> Update([FromQuery] RoleUpdateInput input)
        {
            return await _roleServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultModel> SoftDelete(int id)
        {
            return await _roleServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> BatchSoftDelete(int[] ids)
        {
            return await _roleServices.SoftDeleteAsync(ids);
        }
    }
}
