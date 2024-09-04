using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeDbWrapper.FindAllAsync();
        }

        public async Task<Employee> GetEmployeeByCodeAsync(string employeeCode)
        {
            var employeeByCode = await _employeeDbWrapper.FindAsync(e => e.EmployeeCode == employeeCode);

            return employeeByCode.FirstOrDefault();
        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
            
            var exisitngEmployee = _employeeDbWrapper.Find(e => e.CompanyCode.Equals(employee.CompanyCode)).FirstOrDefault();
            if (exisitngEmployee != null)
            {
                exisitngEmployee.EmployeeName = employee.EmployeeName;
                exisitngEmployee.EmployeeCode = employee.EmployeeCode;
                exisitngEmployee.EmployeeStatus = employee.EmployeeStatus;
                exisitngEmployee.Occupation = employee.Occupation;
                exisitngEmployee.EmailAddress = employee.EmailAddress;
                exisitngEmployee.Phone = employee.Phone;
                exisitngEmployee.LastModified = DateTime.Now;

                return await _employeeDbWrapper.UpdateAsync(exisitngEmployee);

            }

            return await _employeeDbWrapper.InsertAsync(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            return await _employeeDbWrapper.DeleteAsync(e => e.EmployeeCode == employeeCode);
        }
    }
}
