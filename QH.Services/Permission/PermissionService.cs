using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using QH.Core.Cache;
using QH.IRepository;
using QH.Core.Output;
using QH.Models;
using QH.Models.Enums;

namespace QH.Services
{
    using Dapper;
    using QH.Core.Auth;
    using QH.Core.Extensions;
    using QH.Core.Result;
    using QH.Models.ViewModel;
    using System.Text;

    public class PermissionService : IPermissionService
    {
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        private readonly IUser _user;
        

        public PermissionService(
            IMapper mapper,
            ICache cache,
            IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository,
            IUser user
        )
        {
            _mapper = mapper;
            _cache = cache;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _user = user;
        }


        public async Task<IResultModel> PageAsync(PermissionPageInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("where IsDeleted=0 ");
            if (input.Label.NotNull())
            {
                sb.Append(" and Label like @Label ");
            }
            input.Page = 1;
            input.Limit = 999999;
            input.Field = "sort";
            input.Order = "asc";
            var list = await _permissionRepository.GetListPagedAsync(input.Page, input.Limit, sb.ToString(), $"{input.Field} {input.Order}", new { input.Label });
            var total = await _permissionRepository.RecordCountAsync(sb.ToString(), new { input.Label });
            var data = new PageOutput<PermissionPageOutput>()
            {
                Data = _mapper.Map<List<PermissionPageOutput>>(list),
                Count = total
            };
            return ResultModel.Success(data);


        }

        public async Task<IResultModel> GetAsync(int id)
        {
            var result = await _permissionRepository.GetAsync(id);

            return ResultModel.Success(result);
        }

        public async Task<IResultModel> GetGroupAsync(int id)
        {
            var result = await _permissionRepository.GetAsync(id);
            return ResultModel.Success(result);
        }

        public async Task<IResultModel> GetMenuAsync(int id)
        {
            var result = await _permissionRepository.GetAsync(id);
            return ResultModel.Success(result);
        }

        public async Task<IResultModel> GetApiAsync(int id)
        {
            var result = await _permissionRepository.GetAsync(id);
            return ResultModel.Success(result);
        }

        public async Task<IResultModel> GetDotAsync(int id)
        {
            var result = await _permissionRepository.GetAsync(id);
            return ResultModel.Success(result);
        }

        public async Task<IResultModel> ListAsync(string key)
        {
            var data = await _permissionRepository.GetListAsync(key);
            return ResultModel.Success(data);
        }

        public async Task<IResultModel> AddGroupAsync(PermissionAddGroupInput input)
        {
            var entity = _mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity));

            return ResultModel.Result(id > 0);
        }

        public async Task<IResultModel> AddMenuAsync(PermissionAddMenuInput input)
        {
            var entity = _mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity));

            return ResultModel.Result(id > 0);
        }

        public async Task<IResultModel> AddApiAsync(PermissionAddApiInput input)
        {
            var entity = _mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity));

            return ResultModel.Result(id > 0);
        }

        public async Task<IResultModel> AddDotAsync(PermissionAddDotInput input)
        {
            var entity = _mapper.Map<PermissionEntity>(input);
            var id = (await _permissionRepository.InsertAsync(entity));

            return ResultModel.Result(id > 0);
        }

        public async Task<IResultModel> UpdateGroupAsync(PermissionUpdateGroupInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _permissionRepository.GetAsync(input.Id);
                entity = _mapper.Map(input, entity);
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> UpdateMenuAsync(PermissionUpdateMenuInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _permissionRepository.GetAsync(input.Id);
                entity = _mapper.Map(input, entity);
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> UpdateApiAsync(PermissionUpdateApiInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _permissionRepository.GetAsync(input.Id);
                entity = _mapper.Map(input, entity);
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> UpdateDotAsync(PermissionUpdateDotInput input)
        {
            var result = false;
            if (input != null && input.Id > 0)
            {
                var entity = await _permissionRepository.GetAsync(input.Id);
                entity = _mapper.Map(input, entity);
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> DeleteAsync(int id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _permissionRepository.DeleteAsync(id)) > 0;
            }

            return ResultModel.Result(result);
        }


        public async Task<IResultModel> DeleteListAsync(string ids)
        {
            if (ids.IsNull())
                return ResultModel.Result(false);
            var result = await _permissionRepository.DeleteListAsync(" where id in @ids", new { ids =ids.Split(',')});
            return ResultModel.Result(result > 0);
        }

        public async Task<IResultModel> SoftDeleteAsync(int id)
        {
            var result = await _permissionRepository.SoftDeleteAsync(id,_user);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> AssignAsync(PermissionAssignInput input)
        {

            var result = await _permissionRepository.AssignAsync(input);
            //清除权限
            await _cache.DelByPatternAsync(CacheKey.UserPermissions);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> GetPermissionList()
        {
            var permissions = await _permissionRepository.GetListAsync(new { IsDeleted = 0 });
            var apis = permissions
                .Where(a => a.Type == (int)PermissionType.Api)
                .Select(a => new { a.Id, a.ParentId, a.Label });
            var menus = permissions
                .Where(a => a.Type == (int)PermissionType.Group || a.Type == (int)PermissionType.Menu)
                .Select(a => new
                {
                    a.Id,
                    a.ParentId,
                    a.Label,
                    Apis = apis.Where(b => b.ParentId == a.Id).Select(b => new { b.Id, b.Label })
                });
            return ResultModel.Success(menus);
        }


        public async Task<IResultModel> GetPermissionListTree(int roleId = 0)
        {
            var permissions = await _permissionRepository.GetListAsync(new { IsDeleted = 0 }) as List<PermissionEntity>;
            DTreeModel dTree = new DTreeModel();
            dTree.Data = _mapper.Map(permissions, dTree.Data);
            var permissionIds = (await _rolePermissionRepository.GetListAsync(new { roleId })).Select(a => a.PermissionId).ToList();
            for (int i = 0; i < dTree.Data.Count; i++)
            {
                if (permissionIds.Contains(dTree.Data[i].Id))
                    dTree.Data[i].CheckArr = "1";
            }
            return ResultModel.Success(dTree);
        }

        public async Task<IResultModel> GetRolePermissionList(int roleId = 0)
        {
            var permissionIds = (await _rolePermissionRepository.GetListAsync(new { roleId = roleId })).Select(a => a.PermissionId);
            return ResultModel.Success(permissionIds);
        }

        public async Task<IResultModel> GetMenuList(int UserId)
        {
            //用户菜单
            string sql = @"SELECT a.[Id] id , a.[ParentId] , a.[Path] href ,  a.[Label] title , a.[Icon] icon   FROM[sys_permission] a
                        WHERE(a.[IsDeleted] = 0) AND(((a.[Type]) in (1, 2))) AND(exists(SELECT TOP 1 1   FROM[sys_role_permission] b
                           INNER JOIN[sys_user_role] c ON b.[RoleId] = c.[RoleId] AND c.[UserId] = @UserId
                           WHERE(b.[PermissionId] = a.[Id])))
                        ORDER BY a.[ParentId], a.[Sort]";
            var menus = await _rolePermissionRepository.Connection.QueryAsync<MenuModel>(sql, new { UserId }) as List<MenuModel>;
            var rootMenus = menus.Where(o => o.parentid == 0).ToList();
            foreach (var root in rootMenus)
            {
                root.type = 0;
                root.children = menus.Where(o => o.parentid == root.id).ToList();
                for (int i = 0; i < root.children.Count; i++)
                {
                    root.children[i].type = 1;
                    root.children[i].openType = "_iframe";
                }
            }
            return ResultModel.Success(rootMenus);
        }
    }
}
