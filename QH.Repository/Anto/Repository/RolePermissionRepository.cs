
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-07-09 17:45:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： RolePermissionEntityRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using QH.Core.DbHelper;
using QH.Core.Options;
using QH.IRepository;
using QH.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using QH.Core.Auth;

namespace QH.Repository
{
    public partial class RolePermissionRepository:BaseRepository<RolePermissionEntity>, IRolePermissionRepository
    {

       
        public int DeleteLogical(int[] ids)
        {
            string sql = "update RolePermissionEntity set IsDelete=1 where Id in @Ids";
            using var _dbConnection = Connection;
            return _dbConnection.Execute(sql, new
                {
                    Ids = ids
                });
           
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update RolePermissionEntity set IsDelete=1 where Id in @Ids";
            using var _dbConnection = Connection;
            return await _dbConnection.ExecuteAsync(sql, new
                {
                    Ids = ids
                });
            
        }
        public bool UpdateByRolePermission(RolePermissionEntity model)
        {
               var sql = "update ad_role_permission set ";
		   if (model.RoleId != null) sql += " RoleId = @RoleId,";
		   if (model.PermissionId != null) sql += " PermissionId = @PermissionId,";
		   if (model.CreatedUserId != null) sql += " CreatedUserId = @CreatedUserId,";
		   if (model.CreatedUserName != null) sql += " CreatedUserName = @CreatedUserName,";
		   if (model.CreatedTime != null) sql += " CreatedTime = @CreatedTime,";
		   sql = sql.Trim().TrimEnd(',');
		   sql += " where Id = @Id";
            using var _dbConnection = Connection;
            return _dbConnection.Execute(sql, model) > 0 ? true : false;
        }

        public async Task<bool> UpdateByRolePermissionAsync(RolePermissionEntity model)
        {
               var sql = "update ad_role_permission set ";
		   if (model.RoleId != null) sql += " RoleId = @RoleId,";
		   if (model.PermissionId != null) sql += " PermissionId = @PermissionId,";
		   if (model.CreatedUserId != null) sql += " CreatedUserId = @CreatedUserId,";
		   if (model.CreatedUserName != null) sql += " CreatedUserName = @CreatedUserName,";
		   if (model.CreatedTime != null) sql += " CreatedTime = @CreatedTime,";
		   sql = sql.Trim().TrimEnd(',');
		   sql += " where Id = @Id";
            using var _dbConnection = Connection;
            return await _dbConnection.ExecuteAsync(sql, model) > 0 ? true : false;
        }


    }
}