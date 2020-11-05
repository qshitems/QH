
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
    public partial class PermissionRepository:BaseRepository<PermissionEntity>, IPermissionRepository
    {
      

        public int DeleteLogical(int[] ids)
        {
            string sql = "update PermissionEntity set IsDelete=1 where Id in @Ids";
			using var _dbConnection = Connection;
			return _dbConnection.Execute(sql, new
                {
                    Ids = ids
                });
           
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update PermissionEntity set IsDelete=1 where Id in @Ids";
			using var _dbConnection = Connection;
			return await _dbConnection.ExecuteAsync(sql, new
                {
                    Ids = ids
                });
            
        }
        public bool UpdateByPermission(PermissionEntity model)
        {
            string sql = UpdateBysql(model);
            using var _dbConnection = Connection;
            return _dbConnection.Execute(sql, model) > 0 ? true : false;
        }
        public async Task<bool> UpdateByPermissionAsync(PermissionEntity model)
        {
            string sql = UpdateBysql(model);
            using var _dbConnection = Connection;
            return await _dbConnection.ExecuteAsync(sql, model) > 0 ? true : false;
        }

        private static string UpdateBysql(PermissionEntity model)
        {
            var sql = "update ad_permission set ";
            if (model.ParentId != null) sql += " ParentId = @ParentId,";
            if (model.Label != null) sql += " Label = @Label,";
            if (model.Code != null) sql += " Code = @Code,";
            if (model.Type != null) sql += " Type = @Type,";
            if (model.ViewId != null) sql += " ViewId = @ViewId,";
            if (model.ApiId != null) sql += " ApiId = @ApiId,";
            if (model.Path != null) sql += " Path = @Path,";
            if (model.Icon != null) sql += " Icon = @Icon,";
            if (model.Hidden != null) sql += " Hidden = @Hidden,";
            if (model.Enabled != null) sql += " Enabled = @Enabled,";
            if (model.Closable != null) sql += " Closable = @Closable,";
            if (model.Opened != null) sql += " Opened = @Opened,";
            if (model.NewWindow != null) sql += " NewWindow = @NewWindow,";
            if (model.External != null) sql += " External = @External,";
            if (model.Sort != null) sql += " Sort = @Sort,";
            if (model.Description != null) sql += " Description = @Description,";
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
            return sql;
        }

    


    }
}