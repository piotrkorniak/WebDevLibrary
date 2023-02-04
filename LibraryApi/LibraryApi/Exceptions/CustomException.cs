using System;

namespace LibraryApi.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        public CustomException(ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public CustomException(ErrorCode errorCode, string message, Exception innerException) : base(message,
            innerException)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; }
    }
}