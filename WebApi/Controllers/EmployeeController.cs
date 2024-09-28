using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;

namespace WebApi.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{employeeCode}", Name = "GetEmployeeByCode")]
        public async Task<EmployeeDto> Get(string employeeCode)
        {
            _logger.LogInformation($"Handling GET request to retrieve employee with code: {employeeCode}");
            try
            {
                var employee = await _employeeService.GetByCodeAsync(employeeCode);

                if (employee == null)
                {
                    _logger.LogWarning($"No employee found with code: {employeeCode}");
                    return null;
                }

                _logger.LogInformation($"Successfully retrieved employee with code: {employeeCode}");
                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the employee with code: {employeeCode}");
                return null;
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] EmployeeDto employeeDto)
        {
            _logger.LogInformation("Handling POST request to add a new employee.");

            if (employeeDto == null || string.IsNullOrWhiteSpace(employeeDto.EmployeeName))
            {
                _logger.LogWarning("Invalid employee data received.");
                return BadRequest("Employee data is invalid.");
            }
            try
            {
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var success = await _employeeService.AddAsync(employeeInfo);

                if (success)
                {
                    _logger.LogInformation($"Employee '{employeeDto.EmployeeName}' added successfully.");
                    return CreatedAtRoute("GetEmployeeByCode", new { employeeCode = employeeInfo.EmployeeCode }, _mapper.Map<EmployeeDto>(employeeInfo));
                }
                else
                {
                    _logger.LogError($"Failed to add employee '{employeeDto.EmployeeName}'.");
                    return InternalServerError(new Exception("Failed to add employee."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding employee '{employeeDto.EmployeeName}'.");
                return InternalServerError(ex);
            }
        }
    }
}