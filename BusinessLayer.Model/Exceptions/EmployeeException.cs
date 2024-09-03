using System;
namespace BusinessLayer.Model.Exceptions
{
	public class EmployeeException : CustomException
	{
        public string employeeCode { get; set; }
        public EmployeeException(ExceptionReason exceptionReason) : base(exceptionReason)
        {

        }
    }
}

