using QH.IRepository;
using QH.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QH.Services
{
    public partial class JobService : IJobService
    {

        private readonly IJobRepository _jobRepository;
        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<List<JobEntity>> GetListAsync()
        {
            return (List<JobEntity>)await _jobRepository.GetListAsync();
        }

        public async Task<JobEntity> GetAsync( int id)
        {
            return await _jobRepository.GetAsync(id);
        }

        public async Task<int> UpdateStatusAsync(int id, int status)
        {
            var model = new JobEntity
            {
                Id = id,
                Status = status
            };
            return await _jobRepository.UpdateAsync(model);
        }

        public async Task<int> UpdateExecuteAsync(string Group, string Name, DateTime? PreviousFireTime, DateTime? NextFireTime)
        {

            JobEntity model = await _jobRepository.GetAsync(new { Group, Name });
            model.LastOpTime = PreviousFireTime;
            model.NextOpTime = NextFireTime;
            return await _jobRepository.UpdateAsync(model);
        }

    }
}
