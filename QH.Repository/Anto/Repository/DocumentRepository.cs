
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：文档接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-07-09 17:45:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： DocumentEntityRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using QH.IRepository;
using QH.Models;
using Dapper;
using System.Threading.Tasks;
using QH.Core.Auth;

namespace QH.Repository
{
    public partial class DocumentRepository:BaseRepository<DocumentEntity>, IDocumentRepository
    {

		
		public int DeleteLogical(int[] ids)
        {
            string sql = "update DocumentEntity set IsDelete=1 where Id in @Ids";
			using var _dbConnection = Connection;
			return _dbConnection.Execute(sql, new
                {
                    Ids = ids
                });
           
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update DocumentEntity set IsDelete=1 where Id in @Ids";
			using var _dbConnection = Connection;
			return await _dbConnection.ExecuteAsync(sql, new
                {
                    Ids = ids
                });
            
        }
        public bool UpdateByDocument(DocumentEntity model)
        {
               var sql = "update ad_document set ";
		   if (model.ParentId != null) sql += " ParentId = @ParentId,";
		   if (model.Label != null) sql += " Label = @Label,";
		   if (model.Type != null) sql += " Type = @Type,";
		   if (model.Name != null) sql += " Name = @Name,";
		   if (model.Content != null) sql += " Content = @Content,";
		   if (model.Html != null) sql += " Html = @Html,";
		   if (model.Enabled != null) sql += " Enabled = @Enabled,";
		   if (model.Opened != null) sql += " Opened = @Opened,";
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
			using var _dbConnection = Connection;
			return _dbConnection.Execute(sql, model) > 0 ? true : false;
        }

        public async Task<bool> UpdateByDocumentAsync(DocumentEntity model)
        {
               var sql = "update ad_document set ";
		   if (model.ParentId != null) sql += " ParentId = @ParentId,";
		   if (model.Label != null) sql += " Label = @Label,";
		   if (model.Type != null) sql += " Type = @Type,";
		   if (model.Name != null) sql += " Name = @Name,";
		   if (model.Content != null) sql += " Content = @Content,";
		   if (model.Html != null) sql += " Html = @Html,";
		   if (model.Enabled != null) sql += " Enabled = @Enabled,";
		   if (model.Opened != null) sql += " Opened = @Opened,";
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
			using var _dbConnection = Connection;
			return await _dbConnection.ExecuteAsync(sql, model) > 0 ? true : false;
        }


    }
}