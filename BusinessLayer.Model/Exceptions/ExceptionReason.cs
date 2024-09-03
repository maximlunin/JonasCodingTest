using System;
namespace BusinessLayer.Model.Exceptions
{
	public enum ExceptionReason
	{
        NoEmployeeFound,
        CompanyHasEmployees,
        CompanyDoesntExist,
        NoCompanyCodeProvided,
        CompanyNull,
        NoEmployeeCodeProvided,
        NoSitIdProvided,
        SiteIdNoCorrectForCompany,
        CompanyAlreadyExists,
        EmployeeAlreadyExists,
        SiteIdNotCorrectForEmployee,
        SiteIdNotDoesntMatchExistingCompany,
        ExistingEmployeeHasDifferentCompanyCode
    }

    public static class ExceptionReasonDescription
    {
       
        public static string GetDescription(ExceptionReason res)
        {
            switch(res)
            {
                case ExceptionReason.CompanyHasEmployees:
                    return "There are employees in the company, you can use withEmployees parameter if you wish to delete all employees and the company.";
                case ExceptionReason.NoEmployeeFound:
                    return "No such employee exits.";
                case ExceptionReason.CompanyDoesntExist:
                    return "Company for company code doesn't exist.";
                case ExceptionReason.NoEmployeeCodeProvided:
                    return "No Employee Code provided";
                case ExceptionReason.SiteIdNoCorrectForCompany:
                    return "The company code belongs to another site. Wrong SiteId. If you wan to transfer this company to another site. you can delete it and create it with different siteId";
                case ExceptionReason.NoSitIdProvided:
                    return "SiteId is not provided";
                case ExceptionReason.NoCompanyCodeProvided:
                    return "Company code is null or empty";
                case ExceptionReason.CompanyAlreadyExists:
                    return "Company with this code already exists. If you want to update, please use the PUT request";
                case ExceptionReason.EmployeeAlreadyExists:
                    return "Employee with this code already exists. If you want to update, please use the PUT request";
                case ExceptionReason.SiteIdNotCorrectForEmployee:
                    return "The siteId is not correct. Existing employee belongs to a company with different SiteID";
                case ExceptionReason.SiteIdNotDoesntMatchExistingCompany:
                    return "The siteId provided doesn't match with the existing company";
                case ExceptionReason.ExistingEmployeeHasDifferentCompanyCode:
                    return "Employee exists and company code is different than the provided one";
                default:
                    return "";

            }
        }
    }
}

