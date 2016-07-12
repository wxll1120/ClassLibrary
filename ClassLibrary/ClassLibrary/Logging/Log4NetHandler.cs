using System;
using System.Xml;
using System.IO;

namespace ClassLibrary.Logging
{
    public class Log4NetHandler : ILog
    {
        private static log4net.ILog log;

        static Log4NetHandler()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "log4net.config")));

            ProcessConfiguragtion();
        }

        static void ProcessConfiguragtion()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "log4net.config"));

            XmlNode logPolicy = doc.SelectSingleNode("descendant::logPolicy");

            if (logPolicy == null)
                throw new Exception("log4net配置文件中无log策略节点！");

            log = log4net.LogManager.GetLogger(logPolicy.Attributes["name"].Value);
        }

        public void Debug(object message)
        {
            throw new NotImplementedException();
        }

        public void Debug(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(object message)
        {
            throw new NotImplementedException();
        }

        public void Info(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Error(object message, Exception exception)
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
