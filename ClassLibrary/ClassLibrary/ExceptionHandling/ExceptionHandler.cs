using System;

using ClassLibrary.Configuration;

namespace ClassLibrary
{
    public class ExceptionHandler
    {
        

        public static void HandleException(Exception ex)
        {
            try
            {
                ClassLibrary.Logging.LogHandler.Error(ex.Message, ex);
            }
            catch
            { 
                
            }
        }
    }
}
