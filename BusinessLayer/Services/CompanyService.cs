using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.Threading.Tasks;
using Serilog;
using System;
using System.Linq;
using BusinessLayer.Model.Exceptions;
using System.Globalization;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public CompanyService(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IMapper mapper
            , ILogger logger
            )
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            try
            {
                var res = await _companyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyInfo>>(res);

            }
            catch (Exception ex)
            {
                _logger.Error("CompanyService: GetAllCompaniesAsync Exception: " + ex.Message);
                throw ex;
            }
            
        }


        public async Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new CompanyException(ExceptionReason.NoCompanyCodeProvided);
            try
            {
                var result = await _companyRepository.GetByCodeAsync(companyCode);

                if (result == null)
                    throw new CompanyException(ExceptionReason.CompanyDoesntExist);

                return _mapper.Map<CompanyInfo>(result);

            }catch(Exception ex)
            {
                _logger.Error("CompanyService: GetCompanyByCodeAsync companyCode: " + companyCode + "Exception: " + ex.Message);
                throw ex;
            }
        }

        public async Task<bool> SaveCompanyAsync(CompanyInfo companyInfo, bool checkIfExistException = false)
        {

            if (companyInfo == null)
                throw new CompanyException(ExceptionReason.CompanyNull);

            if (string.IsNullOrWhiteSpace(companyInfo.CompanyCode))
                throw new CompanyException(ExceptionReason.NoCompanyCodeProvided);

            if (string.IsNullOrWhiteSpace(companyInfo.SiteId))
                throw new CompanyException(ExceptionReason.NoSitIdProvided);

            try
            {
                var existingCompany = await _companyRepository.GetByCodeAsync(companyInfo.CompanyCode);

                //for post request, if already exists, use put request message
                if (checkIfExistException)
                {                    
                    if(existingCompany != null)
                        throw new CompanyException(ExceptionReason.CompanyAlreadyExists);
                }

                //cannot update site id so if it is different than the supplied siteid, throw an error
                if (existingCompany != null && existingCompany.SiteId != companyInfo.SiteId)
                    throw new CompanyException(ExceptionReason.SiteIdNoCorrectForCompany);
                
                var company = _mapper.Map<Company>(companyInfo);
                company.LastModified = System.DateTime.Now;

                var result = await _companyRepository.SaveCompanyAsync(company);
                return result;
            }
            catch(Exception ex)
            {
                _logger.Error("CompanyService: SaveCompanyAsync companyCode: " + companyInfo.CompanyCode + "\nException: " + ex.Message);
                throw ex;
            }
            
        }

        public async Task<bool> DeleteCompanyAsync(string companyCode, bool withEmployees)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new CompanyException(ExceptionReason.NoCompanyCodeProvided);

            try
            {
                var employeesInCompany = await _employeeRepository.GetEmployeesByCompanyCodeAsync(companyCode);

                //deleting the company with employees will result in inconsistensies so prevent that
                if (withEmployees && (employeesInCompany != null && employeesInCompany.Count() > 0))
                {
                    await _employeeRepository.DeleteEmployeesByCompanyCodeAsync(companyCode);
                }
                else if (employeesInCompany != null && employeesInCompany.Count() > 0)
                {
                    throw new CompanyException(ExceptionReason.CompanyHasEmployees);
                }

                return await _companyRepository.DeleteCompanyAsync(companyCode);

            }
            catch(Exception ex)
            {
                _logger.Error("CompanyService: SaveCompanyAsync companyCode: " + companyCode + "\nException: " + ex.Message);
                throw ex;
            }
            
        }


    }
}
