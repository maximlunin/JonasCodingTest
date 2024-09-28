using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeInfo> GetByCodeAsync(string EmployeeCode);
        Task<bool> AddAsync(EmployeeInfo EmployeeInfo);
    }
}
