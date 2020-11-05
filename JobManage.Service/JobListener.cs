
using QH.Models;
using QH.Repository;
using QH.Services;
using Quartz;
using Quartz.Listener;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class JobListener : JobListenerSupport
    {
        private readonly IJobService _jobService;
        private readonly IJobRunLogService _jobRunLogService;

        public JobListener(IJobService jobService, IJobRunLogService  jobRunLogService)
        {
            _jobService = jobService;
            _jobRunLogService = jobRunLogService;
        }

        public override string Name
        {
            get { return "jobListener"; }
        }

        public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            string group = context.JobDetail.Key.Group;
            string name = context.JobDetail.Key.Name;
         

            if (!JobHelper.IsBaseJob(group, name))
            {

                DateTime fireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.DateTime, TimeZoneInfo.Local);

                DateTime? nextFireTimeUtc = null;
                if (context.NextFireTimeUtc != null)
                {
                    nextFireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local);
                }

                //更新任务运行情况
                await _jobService.UpdateExecuteAsync(group, name, fireTimeUtc, nextFireTimeUtc);

                ////记录运行日志
                //double totalSeconds = context.JobRunTime.TotalSeconds;
                //bool succ = true;
                //string exception = string.Empty;
                //if (jobException != null)
                //{
                //    succ = false;
                //    exception = jobException.ToString();
                //}

                //JobRunLogEntity log = new JobRunLogEntity() { JobGroup = group, JobName = name, StartTime = fireTimeUtc, Succ = succ, Exception = exception };
                //await _jobRunLogService.InsertAsync(log);
            }
        }

    }
}
