﻿using System;

namespace ClassLibrary.ExceptionHandling
{
    public class DataAccessExceptionHandler
    {
        public static void HandlerException(string message, Exception exception)
        {
            if (exception is DataAccessCustomException)
                throw exception;

            throw new DataAccessException(message, exception);
        }
    }
}
