using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Model.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByCodeAsync(string companyCode);
        Task<bool> SaveEmployeeAsync(Employee company);
    }
}
