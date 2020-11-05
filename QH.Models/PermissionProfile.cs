using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Models
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            #region 登录
            CreateMap<UserEntity, AuthLoginOutput>();
            CreateMap<AuthLoginOutput, UserEntity>();
            #endregion

            #region 用户
            //新增
            CreateMap<UserAddInput, UserEntity>();
            CreateMap<UserUpdateInput, UserEntity>();

            //修改
            CreateMap<UserChangePasswordInput, UserEntity>();
            CreateMap<UserUpdateBasicInput, UserEntity>();

            CreateMap<UserGetOutput, UserEntity>();
            CreateMap<UserEntity, UserGetOutput>();

            CreateMap<UserEntity, UserListOutput>();

            #endregion

            #region 操作日志
            CreateMap<OprationLogAddInput, OprationLogEntity>();
            #endregion

            #region 角色
            CreateMap<RoleAddInput, RoleEntity>();
            CreateMap<RoleUpdateInput, RoleEntity>();

            CreateMap<RoleEntity, RoleListOutput>();
            #endregion

            #region 权限
            CreateMap<PermissionAddGroupInput, PermissionEntity>();
            CreateMap<PermissionAddMenuInput, PermissionEntity>();
            CreateMap<PermissionAddApiInput, PermissionEntity>();
            CreateMap<PermissionAddDotInput, PermissionEntity>();

            CreateMap<PermissionUpdateGroupInput, PermissionEntity>();
            CreateMap<PermissionUpdateMenuInput, PermissionEntity>();
            CreateMap<PermissionUpdateApiInput, PermissionEntity>();
            CreateMap<PermissionUpdateDotInput, PermissionEntity>();

            CreateMap<PermissionEntity, PermissionModel>();
            CreateMap<PermissionEntity, PermissionPageOutput>();
            #endregion

            #region 视图
            CreateMap<ViewAddInput, ViewEntity>();
            CreateMap<ViewUpdateInput, ViewEntity>();
            #endregion

            #region 日志
            CreateMap<LoginLogAddInput, LoginLogEntity>();
            #endregion
        }
    }
}
