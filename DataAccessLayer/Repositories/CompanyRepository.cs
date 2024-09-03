using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;


namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
	    private readonly IDbWrapper<Company> _companyDbWrapper;
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

	    public CompanyRepository(IDbWrapper<Company> companyDbWrapper, IDbWrapper<Employee> employeeDbWrapper)
	    {
		    _companyDbWrapper = companyDbWrapper;
            _employeeDbWrapper = employeeDbWrapper;
        }

        public IEnumerable<Company> GetAll()
        {
            return _companyDbWrapper.FindAll();
        }

        public Task<IEnumerable<Company>> GetAllAsync()
        {
            return _companyDbWrapper.FindAllAsync();
        }

        public Company GetByCode(string companyCode)
        {
            return _companyDbWrapper.Find(t => t.CompanyCode.Equals(companyCode))?.FirstOrDefault();
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            var company = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
            return company?.FirstOrDefault();
        }

        public bool SaveCompany(Company company)
        {
            var itemRepo = _companyDbWrapper.Find(t =>
                t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode))?.FirstOrDefault();
            if (itemRepo !=null)
            {
                itemRepo.CompanyName = company.CompanyName;
                itemRepo.AddressLine1 = company.AddressLine1;
                itemRepo.AddressLine2 = company.AddressLine2;
                itemRepo.AddressLine3 = company.AddressLine3;
                itemRepo.Country = company.Country;
                itemRepo.EquipmentCompanyCode = company.EquipmentCompanyCode;
                itemRepo.FaxNumber = company.FaxNumber;
                itemRepo.PhoneNumber = company.PhoneNumber;
                itemRepo.PostalZipCode = company.PostalZipCode;
                itemRepo.LastModified = company.LastModified;
                return _companyDbWrapper.Update(itemRepo);
            }

            return _companyDbWrapper.Insert(company);
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            var itemRepo = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(company.CompanyCode, StringComparison.InvariantCultureIgnoreCase));

            
            if (itemRepo != null)
            {
                var item = itemRepo.FirstOrDefault();
                if(item != null)
                {
                    item.CompanyName = company.CompanyName;
                    item.AddressLine1 = company.AddressLine1;
                    item.AddressLine2 = company.AddressLine2;
                    item.AddressLine3 = company.AddressLine3;
                    item.Country = company.Country;
                    item.EquipmentCompanyCode = company.EquipmentCompanyCode;
                    item.FaxNumber = company.FaxNumber;
                    item.PhoneNumber = company.PhoneNumber;
                    item.PostalZipCode = company.PostalZipCode;
                    item.LastModified = company.LastModified;
                    return await _companyDbWrapper.UpdateAsync(item);
                }                
            }

            return await _companyDbWrapper.InsertAsync(company);
        }

        public bool DeleteCompany(string companyCode)
        {
            return _companyDbWrapper.Delete(t => t.CompanyCode.Equals(companyCode));
        }

        public Task<bool> DeleteCompanyAsync(string companyCode)
        {
            var employeesInCompany = _employeeDbWrapper.FindAsync(x => x.CompanyCode == companyCode);
            return _companyDbWrapper.DeleteAsync(t => t.CompanyCode.Equals(companyCode));
        }

        public Task<bool> DeleteCompanyWithEmployeesAsync(string companyCode)
        {
            var employeesInCompany = _employeeDbWrapper.FindAsync(x => x.CompanyCode == companyCode);
            if (employeesInCompany != null && employeesInCompany.Result.Count() > 0)
            {
                _employeeDbWrapper.Delete(x => x.CompanyCode.Equals(companyCode, StringComparison.InvariantCultureIgnoreCase));
            }
            return _companyDbWrapper.DeleteAsync(t => t.CompanyCode.Equals(companyCode));
        }


    }
}
