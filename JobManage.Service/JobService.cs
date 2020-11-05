using JobManage.Core;
using QH.Models;
using QH.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class JobService : IJob
    {
        private readonly IJobService _jobService;
        private readonly IJobRunLogService _jobRunLogService;
        public JobService(IJobService jobService, IJobRunLogService jobRunLogService)
        {
            _jobService = jobService;
            _jobRunLogService = jobRunLogService;
        }
        public async Task Execute(IJobExecutionContext context)
        {

            var jobs = await _jobService.GetListAsync();

            foreach (var job in jobs)
            {
                switch (job.Status)
                {
                    case (int)JobStatusEnum.ReadyRun:
                        await JobAdd(job, context);
                        break;
                    case (int)JobStatusEnum.ReadyRecover:
                        await JobRecover(job, context);
                        break;
                    case (int)JobStatusEnum.ReadyPause:
                    case (int)JobStatusEnum.Paused:
                        await JobPause(job, context);
                        break;
                    case (int)JobStatusEnum.ReadyDelete:
                    case (int)JobStatusEnum.Deleted:
                        await JobDelete(job, context);
                        break;
                    case (int)JobStatusEnum.Running:
                        await JobRunning(job, context);
                        break;
                }
            }
        }

        /// <summary>
        /// 任务添加
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task JobAdd(JobEntity job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (!await context.Scheduler.CheckExists(jobKey))
                {
                    IJobDetail jobDetail = JobBuilder.Create<BaseJob>()
                            .WithIdentity(jobKey)
                            .UsingJobData("RequestUrl", job.RequestUrl)
                             .UsingJobData("ID", job.Id)
                            .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(group, name)
                        .StartNow()
                        .WithCronSchedule(job.CronExpression)
                        .Build();

                    await context.Scheduler.ScheduleJob(jobDetail, trigger);
                    await _jobService.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Running);
                }
            }
            catch (Exception ex)
            {
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = job.Group, JobName = job.Name, StartTime = DateTime.Now, Succ = false, Exception = "任务重启失败：" + ex.StackTrace });
            }
        }

        /// <summary>
        /// 任务暂停
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        private async Task JobPause(JobEntity job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (await context.Scheduler.CheckExists(jobKey))
                {
                    await context.Scheduler.PauseJob(jobKey);
                }
                await _jobService.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Paused);
            }
            catch (Exception ex)
            {
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = job.Group, JobName = job.Name, StartTime = DateTime.Now, Succ = false, Exception = "任务重启失败：" + ex.StackTrace });
            }
        }

        /// <summary>
        /// 任务重新启动
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        private async Task JobRecover(JobEntity job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (await context.Scheduler.CheckExists(jobKey))
                {
                    await context.Scheduler.ResumeJob(jobKey);
                    await _jobService.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Running);
                }
                else
                {
                    await JobAdd(job, context);
                }
            }
            catch (Exception ex)
            {
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = job.Group, JobName = job.Name, StartTime = DateTime.Now, Succ = false, Exception = "任务重启失败：" + ex.StackTrace });
            }
        }

        /// <summary>
        /// 任务删除
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task JobDelete(JobEntity job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (await context.Scheduler.CheckExists(jobKey))
                {
                    await context.Scheduler.DeleteJob(jobKey);
                }
                await _jobService.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = job.Group, JobName = job.Name, StartTime = DateTime.Now, Succ = false, Exception = "任务重启失败：" + ex.StackTrace });
            }
        }

        /// <summary>
        /// 运行中任务
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task JobRunning(JobEntity job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (!await context.Scheduler.CheckExists(jobKey))
                {
                    await JobAdd(job, context);
                }
            }
            catch (Exception ex)
            {
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = job.Group, JobName = job.Name, StartTime = DateTime.Now, Succ = false, Exception = "任务重启失败：" + ex.StackTrace });
            }
        }

    }
}
