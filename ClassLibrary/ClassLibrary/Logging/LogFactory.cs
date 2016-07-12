using System;
using System.Reflection;

namespace ClassLibrary.Logging
{
    public class LogFactory
    {
        public static ILog CreateLog(string assemblyString)
        {
            ILog log = null;

            string[] assembly = assemblyString.Split(',');

            log = (ILog)Assembly.Load(assembly[1]).CreateInstance(assembly[0]);

            return log;
        }
    }
}
