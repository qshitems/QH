using QH.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QH.Services
{
  
    public interface IJobRunLogService
    {

        Task<int?> InsertAsync(JobRunLogEntity log);
    }
}
