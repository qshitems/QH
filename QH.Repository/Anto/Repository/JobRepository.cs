
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-10-27 14:23:57                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： JobEntityRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using QH.IRepository;
using QH.Models;
using Dapper;
using System.Threading.Tasks;
using QH.Core.Auth;

namespace QH.Repository
{
    public partial class JobRepository:BaseRepository<JobEntity>, IJobRepository
    {
        public JobRepository(IUser user) : base( user)
        {
        }

		public int DeleteLogical(int[] ids)
        {
            string sql = "update JobEntity set IsDelete=1 where Id in @Ids";
          
               return Connection.Execute(sql, new
                {
                    Ids = ids
                });
           
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update JobEntity set IsDelete=1 where Id in @Ids";
           
                return await Connection.ExecuteAsync(sql, new
                {
                    Ids = ids
                });
            
        }
        public bool UpdateBysql(JobEntity model)
        {
               var sql = "update sys_Job set ";
		   if (model.Group != null) sql += " Group = @Group,";
		   if (model.Name != null) sql += " Name = @Name,";
		   if (model.Status != null) sql += " Status = @Status,";
		   if (model.CronExpression != null) sql += " CronExpression = @CronExpression,";
		   if (model.Description != null) sql += " Description = @Description,";
		   if (model.CreateTime != null) sql += " CreateTime = @CreateTime,";
		   if (model.NextOpTime != null) sql += " NextOpTime = @NextOpTime,";
		   if (model.LastOpTime != null) sql += " LastOpTime = @LastOpTime,";
		   if (model.RequestUrl != null) sql += " RequestUrl = @RequestUrl,";
		   sql = sql.Trim().TrimEnd(',');
		   sql += " where Id = @Id";

            return Connection.Execute(sql, model) > 0 ;
        }

        public async Task<bool> UpdateAsyncBysql(JobEntity model)
        {
               var sql = "update sys_Job set ";
		   if (model.Group != null) sql += " Group = @Group,";
		   if (model.Name != null) sql += " Name = @Name,";
		   if (model.Status != null) sql += " Status = @Status,";
		   if (model.CronExpression != null) sql += " CronExpression = @CronExpression,";
		   if (model.Description != null) sql += " Description = @Description,";
		   if (model.CreateTime != null) sql += " CreateTime = @CreateTime,";
		   if (model.NextOpTime != null) sql += " NextOpTime = @NextOpTime,";
		   if (model.LastOpTime != null) sql += " LastOpTime = @LastOpTime,";
		   if (model.RequestUrl != null) sql += " RequestUrl = @RequestUrl,";
		   sql = sql.Trim().TrimEnd(',');
		   sql += " where Id = @Id";

            return await Connection.ExecuteAsync(sql, model) > 0 ;
        }


    }
}