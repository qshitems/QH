
/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：QSH                                            
*│　版    本：1.0                                              
*│　创建时间：2020-10-27 14:23:57                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： QH.Repository                                  
*│　类    名： JobRunLogEntityRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using QH.IRepository;
using QH.Models;
using Dapper;
using System.Threading.Tasks;
using QH.Core.Auth;

namespace QH.Repository
{
    public partial class JobRunLogRepository : BaseRepository<JobRunLogEntity>, IJobRunLogRepository
    {
        public JobRunLogRepository(IUser user) : base(user)
        {
        }

        public int DeleteLogical(int[] ids)
        {
            string sql = "update JobRunLogEntity set IsDelete=1 where Id in @Ids";

            return Connection.Execute(sql, new
            {
                Ids = ids
            });

        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        {
            string sql = "update JobRunLogEntity set IsDelete=1 where Id in @Ids";

            return await Connection.ExecuteAsync(sql, new
            {
                Ids = ids
            });

        }
        public bool UpdateBysql(JobRunLogEntity model)
        {
            var sql = "update sys_JobRunLog set ";
            if (model.JobGroup != null) sql += " JobGroup = @JobGroup,";
            if (model.JobName != null) sql += " JobName = @JobName,";
            if (model.StartTime != null) sql += " StartTime = @StartTime,";
            if (model.Succ != null) sql += " Succ = @Succ,";
            if (model.Exception != null) sql += " Exception = @Exception,";
            sql = sql.Trim().TrimEnd(',');
            sql += " where Id = @Id";

            return Connection.Execute(sql, model) > 0;
        }

        public async Task<bool> UpdateAsyncBysql(JobRunLogEntity model)
        {
            var sql = "update sys_JobRunLog set ";
            if (model.JobGroup != null) sql += " JobGroup = @JobGroup,";
            if (model.JobName != null) sql += " JobName = @JobName,";
            if (model.StartTime != null) sql += " StartTime = @StartTime,";
            if (model.Succ != null) sql += " Succ = @Succ,";
            if (model.Exception != null) sql += " Exception = @Exception,";
            sql = sql.Trim().TrimEnd(',');
            sql += " where Id = @Id";
            return await Connection.ExecuteAsync(sql, model) > 0;
        }


    }
}