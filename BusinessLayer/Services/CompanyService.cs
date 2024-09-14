using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<CompanyInfo>> GetAllCompanies()
        {
            var res = await _companyRepository.GetAll();
            return _mapper.Map<IEnumerable<CompanyInfo>>(res);
        }

        public asynd Task<CompanyInfo> GetCompanyByCode(string companyCode)
        {
            var result = await _companyRepository.GetByCode(companyCode);
            return _mapper.Map<CompanyInfo>(result);
        }

        public async Task<bool> AddCompany(CompanyInfo addCompany)
        {
            _companyRepository.SaveCompany(addCompany);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateCompany(CompanyInfo addCompany)
        {
            _companyRepository.SaveCompany(addCompany);
            await _companyRepository.SaveChangesAsync();
        }

        public async Task DeleteCompany(int id) 
        {
            var company = await _companyRepository.Company.FindAsync(id);
            if(company != null)
            {
                _companyRepository.company.Remove(company);
                 await _context.SaveChangesAsync();         
            }
            else
            {
                 throw new Exception($"No company found with the id {id}");
            }
        }
    }
}
