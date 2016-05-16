using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ISimcpa.Util
{

    public class Logger
    {
        public enum LogLevel
        {
            DEBUG = 0,
            WARNNING = 1,
            ERROR = 2,
            FATAL = 3
        };
        public static log4net.ILog GetLogger(string log_path = "root")
        {
            return log4net.LogManager.GetLogger(log_path);
        }
        /// <summary>
        /// 输出日志到Log4Net, Debug
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="log_path">Log Path, default root</param>
        public static void Debug(string msg, Exception ex = null, string log_path = "root")
        {
            log4net.ILog log = log4net.LogManager.GetLogger(log_path);
            if (ex == null)
            {
                log.Debug(msg);
            }
            else
            {
                log.Debug(msg, ex);
            }
            
        }
        /// <summary>
        /// 输出日志到Log4Net, Warn
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="log_path">Log Path, default root</param>
        public static void Warn(string msg, Exception ex = null, string log_path = "root")
        {
            log4net.ILog log = log4net.LogManager.GetLogger(log_path);
            if (ex == null)
            {
                log.Warn(msg);
            }
            else
            {
                log.Warn(msg, ex);
            }
        }
        /// <summary>
        /// 输出日志到Log4Net, Error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="log_path">Log Path, default root</param>
        public static void Error(string msg, Exception ex=null, string log_path = "root")
        {
            log4net.ILog log = log4net.LogManager.GetLogger(log_path);
            if (ex == null)
            {
                log.Error(msg);
            }
            else
            {
                log.Error(msg, ex);
            }
        }
        /// <summary>
        /// 输出日志到Log4Net, Fatal
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="log_path">Log Path, default root</param>
        public static void Fatal(string msg, Exception ex=null, string log_path = "root")
        {
            log4net.ILog log = log4net.LogManager.GetLogger(log_path);
            if (ex == null)
            {
                log.Fatal(msg);
            }
            else
            {
                log.Fatal(msg, ex);
            }
            
        }

        /// <summary>
        /// 输出日志到Log4Net, Info
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="log_path">Log Path, default root</param>
        public static void Info(string msg, Exception ex=null, string log_path = "root")
        {
            log4net.ILog log = log4net.LogManager.GetLogger(log_path);
            if (ex == null)
            {
                log.Info(msg);
            }
            else
            {
                log.Info(msg, ex);
            }
            
        }

       

    }
}

