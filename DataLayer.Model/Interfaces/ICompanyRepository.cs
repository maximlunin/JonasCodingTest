using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Model.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company> GetByIdAsync(int id);
        Task<Company> GetByCodeAsync(string companyCode);
        Task<bool> SaveCompanyAsync(Company company);
        Task<bool> DeleteAsync(int id);
    }
}
