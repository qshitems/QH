using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Core
{

    public class LoggerHeper
    {
        NLog.Logger _logger;
        private LoggerHeper(NLog.Logger logger)
        {
            _logger = logger;
        }
        public LoggerHeper(string name) : this(LogManager.GetLogger(name))
        {

        }

        public void Info(Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static LoggerHeper Default { get; private set; }
        static LoggerHeper()
        {
            Default = new LoggerHeper(LogManager.GetCurrentClassLogger());
        }

        private static string _path = "";

        /// <summary>
        /// 自定义输出目录，初始化
        /// </summary>
        public void Setting(string path)
        {
            if (_path != path)
            {
                _path = path;
                LogManager.Configuration.Variables["cuspath"] = path + "/";
            }
        }

        /// <summary>
        /// 自定义写日志路径
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="path">写入地址</param>
        /// <returns></returns>
        public void Process(string msg, string path = "")
        {
            _logger.Debug(msg);
        }

        #region Debug
        public void Debug(string msg, params object[] args)
        {
            _logger.Debug(msg, args);
            //LogManager.Shutdown();
        }

        public void Debug(string msg, Exception err)
        {
            _logger.Debug(err, msg);
            //LogManager.Shutdown();
        }
        #endregion

        #region Info
        public void Info(string msg, params object[] args)
        {
            _logger.Info(msg, args);
            //LogManager.Shutdown();
        }

        public void Info(string msg, Exception err)
        {
            _logger.Info(err, msg);
            //LogManager.Shutdown();
        }
        #endregion

        #region Warn
        public void Warn(string msg, params object[] args)
        {
            _logger.Warn(msg, args);
            //LogManager.Shutdown();
        }

        public void Warn(string msg, Exception err)
        {
            _logger.Warn(err, msg);
            //LogManager.Shutdown();
        }
        #endregion

        #region Trace
        public void Trace(string msg, params object[] args)
        {
            _logger.Trace(msg, args);
            //LogManager.Shutdown();
        }

        public void Trace(string msg, Exception err)
        {
            _logger.Trace(err, msg);
            //LogManager.Shutdown();
        }
        #endregion

        #region Error
        public void Error(string msg, params object[] args)
        {
            _logger.Error(msg, args);
            //LogManager.Shutdown();
        }

        public void Error(string msg, Exception err)
        {
            _logger.Error(err, msg);
            //LogManager.Shutdown();
        }
        #endregion

        #region Fatal
        public void Fatal(string msg, params object[] args)
        {
            _logger.Fatal(msg, args);
            //LogManager.Shutdown();
        }

        public void Fatal(string msg, Exception err)
        {
            _logger.Fatal(err, msg);
            //LogManager.Shutdown();
        }
        #endregion
    }

}
