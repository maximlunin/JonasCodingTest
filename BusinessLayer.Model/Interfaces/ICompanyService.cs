using System.Collections.Generic;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyInfo>> GetAllCompanies();
        Task<CompanyInfo?> GetCompanyByCode(string companyCode);
        Task AddCompany(CompanyInfo addCompany);
        Task UpdateCompany(CompanyInfo updateCompany);
        Task DeleteCompany(int id);
    }
}
