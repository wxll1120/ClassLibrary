using System;
using System.Xml;
using System.IO;

using ClassLibrary.Configuration;

namespace ClassLibrary.Logging
{
    public class LogHandler
    {
        private static ClassLibrary.Logging.ILog log;

        private static string logPolicy = string.Empty;

        static LogHandler()
        {
            ProcessConfiguragtion();
        }

        static void ProcessConfiguragtion()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "LogConfig.config"));

            XmlNode logPolicy = doc.SelectSingleNode("descendant::logPolicy");

            if (logPolicy == null)
                throw new Exception("log4net配置文件中无log策略节点！");

            log = ClassLibrary.Logging.LogFactory.CreateLog(logPolicy.Attributes["name"].Value);
        }

        public static void Debug(object message)
        {
            throw new NotImplementedException();
        }

        public static void Debug(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public static void Info(object message)
        {
            throw new NotImplementedException();
        }

        public static void Info(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public static void Warn(object message)
        {
            throw new NotImplementedException();
        }

        public static void Warn(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Fatal(object message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
