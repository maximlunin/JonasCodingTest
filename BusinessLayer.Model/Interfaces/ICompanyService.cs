using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface ICompanyService
    {
        Task <IEnumerable<CompanyInfo>> GetAllCompaniesAsync();
        Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode);
        Task SaveCompanyAsync(CompanyInfo companyInfo);
        Task UpdateCompanyAsync(string companyCode, CompanyInfo companyInfo);
        Task DeleteCompanyAsync(string companyCode);
        
    }
}
