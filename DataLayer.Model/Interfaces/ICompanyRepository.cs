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
        Task<IEnumerable<Company>> GetAll();
        Task<Company> GetByCode(string companyCode);
        Task<bool> SaveCompany(Company company);
        Task DeleteCompany(int id)
    }
}
