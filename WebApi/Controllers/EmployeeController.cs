using AutoMapper;
using BusinessLayer.Model.Models;
using BusinessLayer.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController: ApiController
    {
        private readonly EmployeeService _employeeService;
        private IMapper _mapper;
        private readonly Serilog.ILogger _logger;

        public EmployeeController(EmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = SerilogLogger._logger;
        }
        [HttpGet]
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployee()
        {
            try
            {
                var result = await _employeeService.GetAllEmployeesAsync();

                return _mapper.Map<IEnumerable<EmployeeDto>>(result);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }

        }
        [HttpGet]
        public async Task<EmployeeDto> GetEmployeeByCode(string employeeCode)
        {
            if (String.IsNullOrEmpty(employeeCode))
            {
                throw new Exception(HttpStatusCode.BadRequest.ToString());
            }
            try
            {
                var result = await _employeeService.GetEmployeeByCodeAsync(employeeCode);

                return _mapper.Map<EmployeeDto>(result);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveEmployee([FromBody]EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Employee information not provided");
            }
            try
            {
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);

                await _employeeService.SaveEmployeeAsync(employeeInfo);

                return Ok("Employee information saved");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateEmployee(string employeeCode, [FromBody]EmployeeDto employeeDto)
        {
            if(String.IsNullOrEmpty(employeeCode))
            {
                return BadRequest("Employee Code is null or Empty");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            try
            {
                var exisitngEmployee = _mapper.Map<EmployeeInfo>(employeeDto);

                await _employeeService.UpdateEmployeeAsync(employeeCode, exisitngEmployee);

                return Ok("Employee Information Updated");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerError();
            }

        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteEmployee(string employeeCode)
        {
            if(String.IsNullOrEmpty(employeeCode))
            {
                return BadRequest("Employee Code is Null or Empty");
            }
            try
            {
                await _employeeService.DeleteEmployeeAsync(employeeCode);

                return Ok("Employee Information Deleted");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerError();
            }
        }
    }
}