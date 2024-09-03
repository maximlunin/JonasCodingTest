using System;
namespace WebApi.Models
{
	public static class ExceptionReasons
	{
		public const string NoEmployeeFound = "No such employee exits";
		public const string NoEmployeeCompanyFound = "Employee doesn't have a company associated with it";
		public const string CompanyWithEmployeesCannotDelete = "Company has employees associated with it. It cannot be deleted. If you still want to delete it. Use the parameter force with delete request.";

		public const string Generic = "Something went wrong during the request. Please contact us with the details of your request. Thanks.";
		
	}
}

