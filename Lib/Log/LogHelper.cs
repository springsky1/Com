using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;


namespace Lib.Model
{

    public class LogHelper
    {
        private static log4net.ILog log;
        static LogHelper()
        {
            if (log == null)
            {

                //if (log4net.GlobalContext.Properties["LogName"]==null)
                //    log4net.GlobalContext.Properties["LogName"] = "Log_";
                //  log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));
                // XmlConfigurator.Configure(new Uri(@"\Log4net.config"));
                // log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(@"\log4net.config"));
                // XmlConfigurator.Configure(new Uri(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"\log4net.config")));
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
                log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }


        public void Info(String fileName, object message)
        {
            fileName = "Logs" + "/" + fileName + ".txt";
            //log4net.GlobalContext.Properties["LogName"] = fileName;
            //log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));           
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%d %p %m %n");
            log4net.Appender.FileAppender appender = new log4net.Appender.FileAppender(layout, fileName);
            log4net.Config.BasicConfigurator.Configure(appender);

            ILog logNew = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logNew.Info(message);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }
        public static void Info(object message)
        {
            log.Info(message);
        }
        public static void Debug(object message)
        {
            log.Debug(message);
        }
        public static void Warn(object message)
        {
            log.Warn(message);
        }
    }


}