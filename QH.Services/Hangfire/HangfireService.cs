using NLog;
using QH.Core;
using QH.Core.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Services
{
    public class HangfireService : IHangfireService
    {

        public void HangfireTest()
        {
            Console.WriteLine($"HangfireTest{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}\r\n");
            LoggerHeper.Default.Setting("HangfireTest");
            LoggerHeper.Default.Info($"执行jobs_HangfireTest1_{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}");
        }

        public void HangfireTest1()
        {
            Console.WriteLine($"HangfireTest1{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}\r\n");
            LoggerHeper.Default.Setting("HangfireTest1");
            LoggerHeper.Default.Info($"执行jobs_HangfireTest1_{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}");
        }


        public void HangfireTest2()
        {
            Console.WriteLine($"HangfireTest2{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}\r\n");
            LoggerHeper.Default.Setting("HangfireTest2");
            LoggerHeper.Default.Info($"执行jobs_HangfireTest2_{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}");
        }

        public void HangfireTest3()
        {
            Console.WriteLine($"HangfireTest3{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}\r\n");
            LoggerHeper.Default.Setting("HangfireTest3");
            LoggerHeper.Default.Info($"执行jobs_HangfireTest3_{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}");
        }

        public void HangfireTest4()
        {
            Console.WriteLine($"HangfireTest4{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}\r\n");
            LoggerHeper.Default.Setting("HangfireTest4");
            LoggerHeper.Default.Info($"执行jobs_HangfireTest4_{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}");
        }
    }
}
