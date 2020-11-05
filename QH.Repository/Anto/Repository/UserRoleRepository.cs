
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：用户角色接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-07-09 17:45:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： UserRoleEntityRepository                                      
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
    public partial class UserRoleRepository : BaseRepository<UserRoleEntity>, IUserRoleRepository
    {

       
        public int DeleteLogical(int[] ids)
        {
            string sql = "update UserRoleEntity set IsDelete=1 where Id in @Ids";
            using var _dbConnection = Connection;
            return _dbConnection.Execute(sql, new
            {
                Ids = ids
            });

        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update UserRoleEntity set IsDelete=1 where Id in @Ids";
            using var _dbConnection = Connection;
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Ids = ids
            });

        }
        public bool UpdateByUserRole(UserRoleEntity model)
        {
            var sql = "update ad_user_role set ";
            if (model.UserId != null) sql += " UserId = @UserId,";
            if (model.RoleId != null) sql += " RoleId = @RoleId,";
            if (model.CreatedUserId != null) sql += " CreatedUserId = @CreatedUserId,";
            if (model.CreatedUserName != null) sql += " CreatedUserName = @CreatedUserName,";
            if (model.CreatedTime != null) sql += " CreatedTime = @CreatedTime,";
            sql = sql.Trim().TrimEnd(',');
            sql += " where Id = @Id";
            using var _dbConnection = Connection;
            return _dbConnection.Execute(sql, model) > 0 ? true : false;
        }

        public async Task<bool> UpdateByUserRoleAsync(UserRoleEntity model)
        {
            var sql = "update ad_user_role set ";
            if (model.UserId != null) sql += " UserId = @UserId,";
            if (model.RoleId != null) sql += " RoleId = @RoleId,";
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