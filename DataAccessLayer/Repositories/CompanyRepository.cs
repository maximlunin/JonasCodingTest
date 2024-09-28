using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;
        private readonly ILogger<CompanyRepository> _logger;

        public CompanyRepository(IDbWrapper<Company> companyDbWrapper, ILogger<CompanyRepository> logger)
        {
            _companyDbWrapper = companyDbWrapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                var companies = await _companyDbWrapper.FindAllAsync();
                return companies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all companies.");
                throw;
            }
        }

        public async Task<Company> GetByIdAsync(int siteId)
        {
            try
            {
                var companies = await _companyDbWrapper.FindAsync(t => t.SiteId.Equals(siteId.ToString()));
                var company = companies?.FirstOrDefault();

                if (company == null)
                {
                    _logger.LogWarning($"No company found with id: {siteId}");
                }
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the company with id: {siteId}");
                throw;
            }
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            try
            {
                var companies = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
                var company = companies?.FirstOrDefault();

                if (company == null)
                {
                    _logger.LogWarning($"No company found with code: {companyCode}");
                }
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the company with code: {companyCode}");
                throw;
            }
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            try
            {
                var existingCompany = (await _companyDbWrapper.FindAsync(t =>
                  t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode)))?.FirstOrDefault();

                if (existingCompany != null)
                {
                    existingCompany.CompanyName = company.CompanyName;
                    existingCompany.AddressLine1 = company.AddressLine1;
                    existingCompany.AddressLine2 = company.AddressLine2;
                    existingCompany.AddressLine3 = company.AddressLine3;
                    existingCompany.Country = company.Country;
                    existingCompany.EquipmentCompanyCode = company.EquipmentCompanyCode;
                    existingCompany.FaxNumber = company.FaxNumber;
                    existingCompany.PhoneNumber = company.PhoneNumber;
                    existingCompany.PostalZipCode = company.PostalZipCode;
                    existingCompany.LastModified = company.LastModified;

                    var updated = await _companyDbWrapper.UpdateAsync(existingCompany);
                    return updated;
                }

                var inserted = await _companyDbWrapper.InsertAsync(company);
                return inserted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while saving the company with code: {company.CompanyCode}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _companyDbWrapper.DeleteAsync(t => t.SiteId.Equals(id.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the company with ID: {id}");
                throw;
            }
        }
    }
}
