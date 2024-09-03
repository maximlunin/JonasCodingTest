using System;
using BusinessLayer.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Model.Interfaces
{
	public interface IEmployeeService
	{
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync();
        Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesByCompanyCodeAsync(string companyCode);
        Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo, bool checkIfExistException);
        Task<bool> DeleteEmployeeAsync(string employeeCode);
        Task<bool> DeleteEmployeesByCompanyCodeAsync(string companyCode);
    }
}

