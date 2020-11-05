
using System;
using System.Threading.Tasks;
using QH.Core.Output;
using QH.Core.Result;
using QH.Models;

namespace QH.Services
{
    public partial interface IPermissionService
    {
        Task<IResultModel> PageAsync(PermissionPageInput input);

        Task<IResultModel> GetAsync(int id);

        Task<IResultModel> GetGroupAsync(int id);

        Task<IResultModel> GetMenuAsync(int id);

        Task<IResultModel> GetApiAsync(int id);

        Task<IResultModel> GetDotAsync(int id);

        Task<IResultModel> GetPermissionList();

        Task<IResultModel> GetRolePermissionList(int roleId = 0);

        Task<IResultModel> ListAsync(string key);

        Task<IResultModel> AddGroupAsync(PermissionAddGroupInput input);

        Task<IResultModel> AddMenuAsync(PermissionAddMenuInput input);

        Task<IResultModel> AddApiAsync(PermissionAddApiInput input);

        Task<IResultModel> AddDotAsync(PermissionAddDotInput input);

        Task<IResultModel> UpdateGroupAsync(PermissionUpdateGroupInput input);

        Task<IResultModel> UpdateMenuAsync(PermissionUpdateMenuInput input);

        Task<IResultModel> UpdateApiAsync(PermissionUpdateApiInput input);

        Task<IResultModel> UpdateDotAsync(PermissionUpdateDotInput input);

        Task<IResultModel> DeleteAsync(int id);

        Task<IResultModel> DeleteListAsync(string ids);

        Task<IResultModel> SoftDeleteAsync(int id);

        Task<IResultModel> AssignAsync(PermissionAssignInput input);

        Task<IResultModel> GetMenuList(int UserId);

        Task<IResultModel> GetPermissionListTree(int roleId = 0);
    }
}