using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QH.Api.Attributes;
using QH.Core.Auth;
using QH.Core.Configs;
using QH.Core.Files;
using QH.Core.Helpers;
using QH.Core.Input;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using QH.Services;

namespace QH.Api.Controllers.admin
{
  /// <summary>
  /// 用户管理
  /// </summary>
    public class UserController : AreaController
    {
        private readonly IUser _user;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;
        private readonly IUserService _userServices;

        public UserController(
            IUser user,
            IOptionsMonitor<UploadConfig> uploadConfig,
            UploadHelper uploadHelper,
            IUserService userServices
        )
        {
            _user = user;
            _uploadConfig = uploadConfig.CurrentValue;
            _uploadHelper = uploadHelper;
            _userServices = userServices;
        }

        /// <summary>
        /// 查询用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> GetBasic()
        {
            return await _userServices.GetBasicAsync();
        }

        /// <summary>
        /// 查询单条用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultModel> Get(long id)
        {
            return await _userServices.GetAsync(id);
        }

        /// <summary>
        /// 查询分页用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> GetPage([FromQuery] PageInput<UserListInput> input)
        {
            return await _userServices.PageAsync(input);
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultModel> Add(UserAddInput input)
        {
            return await _userServices.AddAsync(input);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> Update(UserUpdateInput input)
        {
            return await _userServices.UpdateAsync(input);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultModel> SoftDelete(int id)
        {
            return await _userServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> BatchSoftDelete(int[] ids)
        {
            return await _userServices.BatchSoftDeleteAsync(ids);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> ChangePassword(UserChangePasswordInput input)
        {
            return await _userServices.ChangePasswordAsync(input);
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultModel> UpdateBasic(UserUpdateBasicInput input)
        {
            return await _userServices.UpdateBasicAsync(input);
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Login]
        public async Task<IResultModel> AvatarUpload([FromForm] IFormFile file)
        {
            var config = _uploadConfig.Avatar;
            ResultModel<FileInfo> res = await _uploadHelper.UploadAsync(file, config, new { _user.Id }) as ResultModel<FileInfo>;
            if (res.Success)
            {
                return ResultModel.Success(res.Data.FileRelativePath);
            }
            return ResultModel.Failed("上传失败！");
        }
    }
}
