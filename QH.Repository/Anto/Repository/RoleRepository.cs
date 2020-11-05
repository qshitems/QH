
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-07-09 17:45:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： RoleEntityRepository                                      
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
    public partial class RoleRepository:BaseRepository<RoleEntity>, IRoleRepository
    {

       
        public int DeleteLogical(int[] ids)
        {
            string sql = "update ad_role set IsDelete=1 where Id in @Ids";
            using var _dbConnection = Connection;
            return _dbConnection.Execute(sql, new
                {
                    Ids = ids
                });
           
        }

   
        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update ad_role set IsDelete=1 where Id in @Ids";
            using var _dbConnection = Connection;
            return await _dbConnection.ExecuteAsync(sql, new
                {
                    Ids = ids
                });
            
        }
        public bool UpdateByRole(RoleEntity model)
        {
               var sql = "update ad_role set ";
		   if (model.Name != null) sql += " Name = @Name,";
		   if (model.Description != null) sql += " Description = @Description,";
		   if (model.Enabled != null) sql += " Enabled = @Enabled,";
		   if (model.Sort != null) sql += " Sort = @Sort,";
		   if (model.Version != null) sql += " Version = @Version,";
		   if (model.IsDeleted != null) sql += " IsDeleted = @IsDeleted,";
		   if (model.CreatedUserId != null) sql += " CreatedUserId = @CreatedUserId,";
		   if (model.CreatedUserName != null) sql += " CreatedUserName = @CreatedUserName,";
		   if (model.CreatedTime != null) sql += " CreatedTime = @CreatedTime,";
		   if (model.ModifiedUserId != null) sql += " ModifiedUserId = @ModifiedUserId,";
		   if (model.ModifiedUserName != null) sql += " ModifiedUserName = @ModifiedUserName,";
		   if (model.ModifiedTime != null) sql += " ModifiedTime = @ModifiedTime,";
		   sql = sql.Trim().TrimEnd(',');
		   sql += " where Id = @Id";
            using var _dbConnection = Connection;
            return _dbConnection.Execute(sql, model) > 0 ? true : false;
        }

        public async Task<bool> UpdateByRoleAsync(RoleEntity model)
        {
               var sql = "update ad_role set ";
		   if (model.Name != null) sql += " Name = @Name,";
		   if (model.Description != null) sql += " Description = @Description,";
		   if (model.Enabled != null) sql += " Enabled = @Enabled,";
		   if (model.Sort != null) sql += " Sort = @Sort,";
		   if (model.Version != null) sql += " Version = @Version,";
		   if (model.IsDeleted != null) sql += " IsDeleted = @IsDeleted,";
		   if (model.CreatedUserId != null) sql += " CreatedUserId = @CreatedUserId,";
		   if (model.CreatedUserName != null) sql += " CreatedUserName = @CreatedUserName,";
		   if (model.CreatedTime != null) sql += " CreatedTime = @CreatedTime,";
		   if (model.ModifiedUserId != null) sql += " ModifiedUserId = @ModifiedUserId,";
		   if (model.ModifiedUserName != null) sql += " ModifiedUserName = @ModifiedUserName,";
		   if (model.ModifiedTime != null) sql += " ModifiedTime = @ModifiedTime,";
		   sql = sql.Trim().TrimEnd(',');
		   sql += " where Id = @Id";
            using var _dbConnection = Connection;
            return await _dbConnection.ExecuteAsync(sql, model) > 0 ? true : false;
        }


    }
}