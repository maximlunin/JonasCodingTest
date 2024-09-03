using System;
namespace BusinessLayer.Model.Exceptions
{
	public class CustomException : Exception
	{
        public ExceptionReason ExceptionReason { get; set; }
        

        public CustomException(ExceptionReason exceptionReason, string message): base(message) 
		{
            ExceptionReason = exceptionReason;
        }

        public CustomException(ExceptionReason exceptionReason)
        {
            ExceptionReason = exceptionReason;
        }
    }
}

