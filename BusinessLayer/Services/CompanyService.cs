using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CompanyInfo>> GetAllAsync()
        {
            try
            {
                var companies = await _companyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyInfo>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all companies.");
                throw;
            }
        }

        public async Task<CompanyInfo> GetByCodeAsync(string companyCode)
        {
            try
            {
                var company = await _companyRepository.GetByCodeAsync(companyCode);
                if (company == null)
                {
                    _logger.LogWarning($"Company with code: {companyCode} not found.");
                    return null;
                }
                return _mapper.Map<CompanyInfo>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the company with code: {companyCode}");
                throw;
            }
        }

        public async Task<CompanyInfo> GetByIdAsync(int id)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null)
                {
                    _logger.LogWarning($"Company with id: {id} not found.");
                    return null;
                }
                return _mapper.Map<CompanyInfo>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the company with id: {id}");
                throw;
            }
        }

        public async Task<bool> AddAsync(CompanyInfo companyInfo)
        {
            try
            {
                var company = _mapper.Map<Company>(companyInfo);
                var success = await _companyRepository.SaveCompanyAsync(company);
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a company.");
                throw;
            }

        }

        public async Task<bool> UpdateAsync(int id, CompanyInfo companyInfo)
        {
            try
            {
                var existingCompany = await _companyRepository.GetByIdAsync(id);
                if (existingCompany == null)
                {
                    return false;
                }

                var company = _mapper.Map<Company>(companyInfo);
                var success = await _companyRepository.SaveCompanyAsync(company);
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the company with ID: {id}");
                throw;
            }

        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existingCompany = await _companyRepository.GetByIdAsync(id);
                if (existingCompany == null)
                {
                    return false;
                }
                var success = await _companyRepository.DeleteAsync(id);
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the company with ID: {id}");
                throw;
            }

        }

    }
}
