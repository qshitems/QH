
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：操作日志接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-07-09 17:45:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： OprationLogEntityRepository                                      
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
    public partial class OprationLogRepository:BaseRepository<OprationLogEntity>, IOprationLogRepository
    {

		public int DeleteLogical(int[] ids)
        {
            string sql = "update OprationLogEntity set IsDelete=1 where Id in @Ids";
			using var _dbConnection = Connection;
			return _dbConnection.Execute(sql, new
                {
                    Ids = ids
                });
           
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update OprationLogEntity set IsDelete=1 where Id in @Ids";
			using var _dbConnection = Connection;
			return await _dbConnection.ExecuteAsync(sql, new
                {
                    Ids = ids
                });
            
        }
        public bool UpdateByOprationLog(OprationLogEntity model)
        {
               var sql = "update ad_opration_log set ";
		   if (model.ApiLabel != null) sql += " ApiLabel = @ApiLabel,";
		   if (model.ApiPath != null) sql += " ApiPath = @ApiPath,";
		   if (model.ApiMethod != null) sql += " ApiMethod = @ApiMethod,";
		   if (model.NickName != null) sql += " NickName = @NickName,";
		   if (model.IP != null) sql += " IP = @IP,";
		   if (model.Browser != null) sql += " Browser = @Browser,";
		   if (model.Os != null) sql += " Os = @Os,";
		   if (model.Device != null) sql += " Device = @Device,";
		   if (model.BrowserInfo != null) sql += " BrowserInfo = @BrowserInfo,";
		   if (model.ElapsedMilliseconds != null) sql += " ElapsedMilliseconds = @ElapsedMilliseconds,";
		   if (model.Status != null) sql += " Status = @Status,";
		   if (model.Msg != null) sql += " Msg = @Msg,";
		   if (model.Result != null) sql += " Result = @Result,";
		   if (model.Params != null) sql += " Params = @Params,";
		   if (model.CreatedUserId != null) sql += " CreatedUserId = @CreatedUserId,";
		   if (model.CreatedUserName != null) sql += " CreatedUserName = @CreatedUserName,";
		   if (model.CreatedTime != null) sql += " CreatedTime = @CreatedTime,";
		   sql = sql.Trim().TrimEnd(',');
		   sql += " where Id = @Id";
			using var _dbConnection = Connection;
			return _dbConnection.Execute(sql, model) > 0 ? true : false;
        }

        public async Task<bool> UpdateByOprationLogAsync(OprationLogEntity model)
        {
               var sql = "update ad_opration_log set ";
		   if (model.ApiLabel != null) sql += " ApiLabel = @ApiLabel,";
		   if (model.ApiPath != null) sql += " ApiPath = @ApiPath,";
		   if (model.ApiMethod != null) sql += " ApiMethod = @ApiMethod,";
		   if (model.NickName != null) sql += " NickName = @NickName,";
		   if (model.IP != null) sql += " IP = @IP,";
		   if (model.Browser != null) sql += " Browser = @Browser,";
		   if (model.Os != null) sql += " Os = @Os,";
		   if (model.Device != null) sql += " Device = @Device,";
		   if (model.BrowserInfo != null) sql += " BrowserInfo = @BrowserInfo,";
		   if (model.ElapsedMilliseconds != null) sql += " ElapsedMilliseconds = @ElapsedMilliseconds,";
		   if (model.Status != null) sql += " Status = @Status,";
		   if (model.Msg != null) sql += " Msg = @Msg,";
		   if (model.Result != null) sql += " Result = @Result,";
		   if (model.Params != null) sql += " Params = @Params,";
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