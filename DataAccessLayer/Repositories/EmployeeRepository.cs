using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using Serilog;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;
        public readonly ILogger _logger;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper, ILogger logger)
        {
            _employeeDbWrapper = employeeDbWrapper;
            _logger = logger;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeDbWrapper.FindAll();
        }

        public Task<IEnumerable<Employee>> GetAllAsync()
        {
            return _employeeDbWrapper.FindAllAsync();
        }

        public Employee GetByCode(string employeeCode)
        {
            return _employeeDbWrapper.Find(t => t.Equals(employeeCode))?.FirstOrDefault();
        }

        public async Task<Employee> GetByCodeAsync(string employeeCode)
        {
            var employee = await _employeeDbWrapper.FindAsync(t => t.EmployeeCode.Equals(employeeCode, System.StringComparison.InvariantCultureIgnoreCase));
            return employee.FirstOrDefault();
        }

        public bool SaveEmployee(Employee employee)
        {
            var itemRepo = _employeeDbWrapper.Find(t => 
                t.EmployeeCode.Equals(employee.EmployeeCode, System.StringComparison.InvariantCultureIgnoreCase) && t.CompanyCode.Equals(employee.CompanyCode, System.StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault();
            if (itemRepo != null)
            {
                itemRepo.EmailAddress = employee.EmailAddress;
                itemRepo.EmployeeName = employee.EmployeeName;
                itemRepo.EmployeeStatus = employee.EmployeeStatus;
                itemRepo.Occupation = employee.Occupation;
                itemRepo.Phone = employee.Phone;
                itemRepo.SiteId = employee.SiteId;
                itemRepo.LastModified = employee.LastModified;
                return _employeeDbWrapper.Update(itemRepo);
            }

            return _employeeDbWrapper.Insert(itemRepo);
        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
            var itemRepo = await _employeeDbWrapper.FindAsync(t =>
                t.CompanyCode.Equals(employee.CompanyCode, System.StringComparison.InvariantCultureIgnoreCase) && t.EmployeeCode.Equals(employee.EmployeeCode, System.StringComparison.InvariantCultureIgnoreCase));

            if (itemRepo != null && itemRepo.Count() > 0)
            {
                var item = itemRepo.FirstOrDefault();
                if (item != null)
                {
                    item.EmailAddress = employee.EmailAddress;
                    item.EmployeeName = employee.EmployeeName;
                    item.EmployeeStatus = employee.EmployeeStatus;
                    item.Occupation = employee.Occupation;
                    item.Phone = employee.Phone;
                    item.SiteId = employee.SiteId;
                    item.LastModified = employee.LastModified;
                    return _employeeDbWrapper.Update(item);
                }
            }

            return await _employeeDbWrapper.InsertAsync(employee);
        }

        public bool DeleteEmployee(string employeeCode)
        {
            return _employeeDbWrapper.Delete(t => t.EmployeeCode.Equals(employeeCode));
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            return await _employeeDbWrapper.DeleteAsync(t => t.EmployeeCode.Equals(employeeCode));
        }

        public async Task<bool> DeleteEmployeesByCompanyCodeAsync(string companyCode)
        {
            return await _employeeDbWrapper.DeleteAsync(t => t.CompanyCode.Equals(companyCode));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByCompanyCodeAsync(string companyCode)
        {
            return await _employeeDbWrapper.FindAsync(x => x.CompanyCode.Equals(companyCode));
        }
    }
}
