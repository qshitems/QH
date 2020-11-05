using QH.IRepository;
using QH.Models;
using QH.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QH.Services
{
   
    public partial class JobRunLogService : IJobRunLogService
    {
        private readonly IJobRunLogRepository  _jobRunLogRepository;
        public JobRunLogService(IJobRunLogRepository  jobRunLogRepository)
        {
            _jobRunLogRepository = jobRunLogRepository;
        }
        public async Task<int?> InsertAsync(JobRunLogEntity log)
        {
            return await _jobRunLogRepository.InsertAsync(log);
        }

   

    }
}
