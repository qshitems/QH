
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：权限接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-07-09 17:45:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： PermissionEntityRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/

using QH.IRepository;
using QH.Models;
using Dapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using QH.Core.Extensions;
using QH.Core.Auth;

namespace QH.Repository
{
    public partial class PermissionRepository : BaseRepository<PermissionEntity>, IPermissionRepository
    {


        private readonly IUser _user;

        public PermissionRepository(IUser user)
        {
            _user = user;
        }


        public async Task<IEnumerable<PermissionListOutput>> GetListAsync(string key)
        {

            var sql = @"SELECT a.[Id] , a.[ParentId],  a.[Label] , a.[Type] , a.[Path] , a.[Description] , a.[Icon] , a.[Opened] , a.[Hidden] , a.[Enabled] 
                FROM [ad_permission] a
                WHERE (a.[IsDeleted] = 0) ";
            if (key.NotNull())
            {
                sql += " and ( a.[Label] like '%@key%' or a.Path like'%@key%')";
            }
            sql += " ORDER BY a.[ParentId], a.[Sort]";
            using var _dbConnection = Connection;
            return await _dbConnection.QueryAsync<PermissionListOutput>(sql, new { key });
        }







        public async Task<bool> AssignAsync(PermissionAssignInput input)
        {
            var sql = @"SELECT a.[PermissionId] FROM[ad_role_permission] a WHERE(a.[RoleId] = @RoleId)";
            try
            {

                BeginTrans();
                //查询角色权限
                var permissionIds = await Transaction.Connection.QueryAsync<int>(sql, new {input.RoleId }, Transaction);
                //批量删除权限
                var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
                if (deleteIds.Count() > 0)
                {
                    sql = "DELETE FROM  ad_role_permission WHERE RoleId = @RoleId AND PermissionId in @PermissionId";
                    await Transaction.Connection.ExecuteAsync(sql, new { input.RoleId, PermissionId = deleteIds }, Transaction);
                }
                //批量插入权限
                var insertRolePermissions = new List<RolePermissionEntity>();
                var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));
                if (insertPermissionIds.Count() > 0)
                {
                    foreach (var permissionId in insertPermissionIds)
                    {
                        //insertRolePermissions.Add(new RolePermissionEntity()
                        //{
                        //    RoleId = input.RoleId,
                        //    PermissionId = permissionId,
                        //    CreatedTime = DateTime.UtcNow,
                        //    CreatedUserId = _user.Id,
                        //    CreatedUserName = _user.Name
                        //});

                        await Transaction.Connection.InsertAsync(new RolePermissionEntity()
                        {
                            RoleId = input.RoleId,
                            PermissionId = permissionId,
                            CreatedTime = DateTime.UtcNow,
                            CreatedUserId = _user.Id,
                            CreatedUserName = _user.Name
                        }, Transaction);

                    }
                    //  await Transaction.Connection.InsertAsync(insertRolePermissions, Transaction);
                }
                Commit();
                return true;
            }
            catch (Exception ex)
            {
                Rollback();
            }

            return false;
        }


    }
}