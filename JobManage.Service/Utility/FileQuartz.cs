using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JobManage.Service.Utility
{
    public class FileQuartz
    {
        private static string _rootPath { get; set; }

        private static string _logPath { get; set; }
        /// <summary>
        /// 创建作业所在根目录及日志文件夹 
        /// </summary>
        /// <returns></returns>
        public static string CreateQuartzRootPath()
        {
            if (!string.IsNullOrEmpty(_rootPath))
                return _rootPath;
            _rootPath = $"{Directory.GetCurrentDirectory()}\\QuartzSettings\\";
            _rootPath = _rootPath.ReplacePath();
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
            _logPath = _rootPath + "logs" + "\\";
            _logPath = _logPath.ReplacePath();
            //生成日志文件夹
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
            return _rootPath;
        }

        /// <summary>
        /// 初始化作业日志文件,以txt作为文件
        /// </summary>
        /// <param name="groupJobName"></param>
        public static void InitGroupJobFileLog(string groupJobName)
        {
            string jobFile = _logPath + groupJobName;
            jobFile = jobFile.ReplacePath();
            if (!File.Exists(jobFile))
            {
                File.Create(jobFile);
            }
        }



   


        public static string RootPath
        {
            get { return _rootPath; }
        }

        public static string LogPath
        {
            get { return _logPath; }
        }
    }
}
