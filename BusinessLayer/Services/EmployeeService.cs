using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.Threading.Tasks;
using BusinessLayer.Model.Exceptions;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            var res = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeInfo>>(res);
        }

        public async Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode)
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                throw new EmployeeException(ExceptionReason.NoEmployeeCodeProvided);

            var result = await _employeeRepository.GetByCodeAsync(employeeCode);

            //check if employee exists
            if (result == null)
                throw new EmployeeException(ExceptionReason.NoEmployeeFound);

            return _mapper.Map<EmployeeInfo>(result);
         }
        

        public async Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo, bool checkIfExistException)
        {
            //just to check if companyExists
            var existingCompany = _companyRepository.GetByCode(employeeInfo.CompanyCode);

            if (existingCompany == null)
                throw new CompanyException(ExceptionReason.CompanyDoesntExist);

            var existingEmployee = await _employeeRepository.GetByCodeAsync(employeeInfo.EmployeeCode);

            //for post request only create throw exception if already exists
            if (checkIfExistException && existingEmployee != null)
                throw new EmployeeException(ExceptionReason.EmployeeAlreadyExists);

            //company's siteID doesn't match with provided siteId for employee
            if (existingCompany != null && !(existingCompany.SiteId.Equals(employeeInfo.SiteId, System.StringComparison.InvariantCultureIgnoreCase)))
                throw new EmployeeException(ExceptionReason.SiteIdNotDoesntMatchExistingCompany);

            //siteid of employee object should match with siteid from company
            if (existingEmployee != null && !existingEmployee.SiteId.Equals(employeeInfo.SiteId,System.StringComparison.InvariantCultureIgnoreCase))
                throw new EmployeeException(ExceptionReason.SiteIdNotCorrectForEmployee);

            //Provided Companycode should be same as the one with existing companyCode
            if (existingEmployee != null && !existingEmployee.CompanyCode.Equals(employeeInfo.CompanyCode, System.StringComparison.InvariantCultureIgnoreCase))
                throw new EmployeeException(ExceptionReason.ExistingEmployeeHasDifferentCompanyCode);


            var employee = _mapper.Map<Employee>(employeeInfo);
            employee.LastModified = System.DateTime.Now;

            var result = await _employeeRepository.SaveEmployeeAsync(employee);
            return result;
        }


        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            var existingEmployee = await _employeeRepository.GetByCodeAsync(employeeCode);

            if(existingEmployee == null)
            {
                throw new EmployeeException(ExceptionReason.NoEmployeeFound);
            }

            return await _employeeRepository.DeleteEmployeeAsync(employeeCode);
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByCompanyCodeAsync(string companyCode)
        {
            var employees = await _employeeRepository.GetEmployeesByCompanyCodeAsync(companyCode);
            return _mapper.Map<IEnumerable<EmployeeInfo>>(employees);
        }

        public async Task<bool> DeleteEmployeesByCompanyCodeAsync(string companyCode)
        {
            return await _employeeRepository.DeleteEmployeesByCompanyCodeAsync(companyCode);
        }

    }
}


