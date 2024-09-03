using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Model.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAll();
        Company GetByCode(string companyCode);
        bool SaveCompany(Company company);
        bool DeleteCompany(string companyCode);

        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company> GetByCodeAsync(string companyCode);
        Task<bool> SaveCompanyAsync(Company company);
        Task<bool> DeleteCompanyAsync(string companyCode);
    }
}
