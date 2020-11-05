

using QH.Core.Output;
using QH.Core.Result;
using QH.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QH.Services
{
    public interface IJobService
    {
        Task<List<JobEntity>> GetListAsync();
        Task<int> UpdateStatusAsync(int id, int status);
        Task<int> UpdateExecuteAsync(string Group, string Name, DateTime? PreviousFireTime, DateTime? NextFireTime);

        Task<JobEntity> GetAsync(int id);
    }
}