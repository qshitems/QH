using JobManage.Service.Utility;
using QH.Core;
using QH.Core.Helpers;
using QH.Models;
using QH.Services;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class BaseJob : IJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IJobService _jobService;
        private readonly IJobRunLogService _jobRunLogService;

        public BaseJob(IHttpClientFactory clientFactory, IJobService jobService, IJobRunLogService jobRunLogService)
        {
            _clientFactory = clientFactory;
            _jobService = jobService;
            _jobRunLogService = jobRunLogService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var sw = new Stopwatch();
            sw.Start();
            DateTime dateTime = DateTime.Now;
            var id = context.JobDetail.JobDataMap.GetIntValue("ID");
          //  var RequestUrl = context.JobDetail.JobDataMap.GetString("RequestUrl");

            JobEntity job = await _jobService.GetAsync(id);
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (job == null)
            {
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = trigger.Group, JobName = trigger.Name, StartTime = DateTime.Now, Succ = false, Exception = "未到找作业或可能被移除" });
                return;
            }
            Utility.FileHelper.WriteFile(FileQuartz.LogPath + trigger.Group, $"{trigger.Name}.txt", $"作业[{job.Name}]开始:{ DateTime.Now:yyyy-MM-dd HH:mm:sss}", true);
            if (string.IsNullOrEmpty(job.RequestUrl) || job.RequestUrl == "/")
            {
                Utility.FileHelper.WriteFile(FileQuartz.LogPath + trigger.Group, $"{trigger.Name}.txt", $"{ DateTime.Now:yyyy-MM-dd HH:mm:sss}未配置url,", true);
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = trigger.Group, JobName = trigger.Name, StartTime = DateTime.Now, Succ = false, Exception = "未配置url" });
                return;
            }
            try
            {


            string httpMessage="";
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(job.RequestType?.ToLower() == "get" ? HttpMethod.Get : HttpMethod.Post, job.RequestUrl);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(job.AuthKey)
                && !string.IsNullOrEmpty(job.AuthValue))
            {
                headers.Add(job.AuthKey.Trim(), job.AuthValue.Trim());
            }
            if (headers != null &&headers.Count>0)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                httpMessage= await response.Content.ReadAsStringAsync();
            }
            sw.Stop();
            await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = job.Group, JobName = job.Name, StartTime = DateTime.Now, Succ = response.IsSuccessStatusCode, Exception = response.IsSuccessStatusCode?httpMessage: response.ReasonPhrase, RequestMessage=response.ToJson(), StatusCode=(int) response.StatusCode, TotalSeconds=(int)sw.ElapsedMilliseconds });
            string logContent = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}_{dateTime:yyyy-MM-dd HH:mm:ss}_{(string.IsNullOrEmpty(httpMessage) ? "" : httpMessage)}\r\n";
            Utility.FileHelper.WriteFile(FileQuartz.LogPath + job.Group + "\\", $"{job.Name}.txt", logContent, true);
            Console.WriteLine(trigger.FullName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + " " + httpMessage);

            }
            catch (Exception ex)
            {
                await _jobRunLogService.InsertAsync(new JobRunLogEntity() { JobGroup = job.Group, JobName = job.Name, StartTime = DateTime.Now, Succ = false, Exception = ex.Message,  TotalSeconds = (int)sw.ElapsedMilliseconds });

            }
        }

    }
}
