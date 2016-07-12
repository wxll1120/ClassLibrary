using System;
using System.Runtime.Serialization;

namespace ClassLibrary.ExceptionHandling
{
    public class UserInterfaceCustomException : BaseException, ISerializable
    {
        public UserInterfaceCustomException()
            : base()
        {

        }

        public UserInterfaceCustomException(string message)
            : base(message)
        {

        }

        public UserInterfaceCustomException(string message,
            System.Exception innerException)
            : base(message, innerException)
        {

        }

        public UserInterfaceCustomException(SerializationInfo serializationInfo,
            StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}