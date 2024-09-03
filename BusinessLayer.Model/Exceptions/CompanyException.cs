using System;
namespace BusinessLayer.Model.Exceptions
{
	public class CompanyException : CustomException
	{
        public string companyCode { get; set; }
        public CompanyException(ExceptionReason exceptionReason) : base(exceptionReason)
		{
			
		}
	}
}

