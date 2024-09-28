using System.Linq;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _EmployeeDbWrapper;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(IDbWrapper<Employee> EmployeeDbWrapper, ILogger<EmployeeRepository> logger)
        {
            _EmployeeDbWrapper = EmployeeDbWrapper;
            _logger = logger;
        }

        public async Task<Employee> GetByCodeAsync(string EmployeeCode)
        {
            try
            {
                var employees = await _EmployeeDbWrapper.FindAsync(t => t.EmployeeCode.Equals(EmployeeCode));
                var Employee = employees?.FirstOrDefault();

                if (Employee == null)
                {
                    _logger.LogWarning($"No Employee found with code: {EmployeeCode}");
                }
                return Employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the Employee with code: {EmployeeCode}");
                throw;
            }
        }

        public async Task<bool> SaveEmployeeAsync(Employee Employee)
        {
            try
            {
                var inserted = await _EmployeeDbWrapper.InsertAsync(Employee);
                return inserted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while saving the Employee with code: {Employee.EmployeeCode}");
                throw;
            }
        }
    }
}
