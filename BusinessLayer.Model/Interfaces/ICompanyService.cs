using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyInfo>> GetAllAsync();
        Task<CompanyInfo> GetByCodeAsync(string companyCode);
        Task<CompanyInfo> GetByIdAsync(int id);
        Task<bool> AddAsync(CompanyInfo companyInfo);
        Task<bool> UpdateAsync(int id, CompanyInfo companyInfo);
        Task<bool> DeleteAsync(int id);
    }
}
