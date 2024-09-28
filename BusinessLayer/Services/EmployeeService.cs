using BusinessLayer.Model.Interfaces;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EmployeeService(IEmployeeRepository EmployeeRepository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _EmployeeRepository = EmployeeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeInfo> GetByCodeAsync(string EmployeeCode)
        {
            try
            {
                var Employee = await _EmployeeRepository.GetByCodeAsync(EmployeeCode);
                if (Employee == null)
                {
                    _logger.LogWarning($"Employee with code: {EmployeeCode} not found.");
                    return null;
                }
                return _mapper.Map<EmployeeInfo>(Employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the Employee with code: {EmployeeCode}");
                throw;
            }
        }

        public async Task<bool> AddAsync(EmployeeInfo EmployeeInfo)
        {
            try
            {
                var Employee = _mapper.Map<Employee>(EmployeeInfo);
                var success = await _EmployeeRepository.SaveEmployeeAsync(Employee);
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a Employee.");
                throw;
            }
        }
    }
}
