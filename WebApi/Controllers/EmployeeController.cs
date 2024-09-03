using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Services.Description;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;
using Serilog;
using BusinessLayer.Model.Exceptions;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeService employeeService, ICompanyService companyService, IMapper mapper, ILogger logger)
        {
            _employeeService = employeeService;
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }
        // GET api/<controller>
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var items = await _employeeService.GetAllEmployeesAsync();
                var employees = _mapper.Map<IEnumerable<EmployeeDto>>(items);
                foreach (var employee in employees)
                {
                    var company = await _companyService.GetCompanyByCodeAsync(employee.CompanyCode);
                    employee.CompanyName = company.CompanyName;                  
                }

                return Ok(employees);


            }catch(Exception ex)
            {
                _logger.Error("EmployeeController GetAll, Exception : " + ex.Message);
                return Content(HttpStatusCode.InternalServerError, ExceptionReasons.Generic);
            }
        }

        // GET api/<controller>/5
        public async Task<IHttpActionResult> Get(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Employee code was null or empty");
            try
            {
                var item = await _employeeService.GetEmployeeByCodeAsync(code);

                var company = await _companyService.GetCompanyByCodeAsync(item.CompanyCode);
                if(company == null)
                    return Content(HttpStatusCode.InternalServerError, ExceptionReasons.NoEmployeeCompanyFound);

                EmployeeDto result = _mapper.Map<EmployeeDto>(item);
                result.CompanyName = company.CompanyName;

                return Ok(result);
            }
            catch(EmployeeException ex)
            {
                return Content(HttpStatusCode.NotFound, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
            }
            catch (Exception ex)
            {
                _logger.Error("Error found for " + code + " in Get for EmployeeController.\nException:" + ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionReasons.Generic));
            }

        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post([FromBody] EmployeeDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var employee = _mapper.Map<EmployeeInfo>(value);
                
                await _employeeService.SaveEmployeeAsync(employee, true);
                
                return Created(employee.EmployeeCode, employee);
            }
            catch (CompanyException ex)
            {
                return Content(HttpStatusCode.Conflict, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));

            }catch(EmployeeException ex)
            {
                return Content(HttpStatusCode.Conflict, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
            }
            catch (Exception ex)
            {
                _logger.Error("Error found for employeeCode: " + value.EmployeeCode + " in Get for EmployeeController.\nException:" + ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionReasons.Generic));
            }
        }

        // PUT api/<controller>/{companyCode}
        public async Task<IHttpActionResult> Put(string code, [FromBody] EmployeeDto value)
        {
            if (string.IsNullOrWhiteSpace(code))
                BadRequest("Provided code was empty or null");

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            if (!code.Equals(value.EmployeeCode))
                BadRequest("Changing employeecode for existing employee is not supported");

            try
            {
                var emp = _mapper.Map<EmployeeInfo>(value);

                await _employeeService.SaveEmployeeAsync(emp, false);

                return Content(HttpStatusCode.OK, code);
            }
            catch (CompanyException ex)
            {
                return Content(HttpStatusCode.Conflict, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));

            }
            catch (EmployeeException ex)
            {
                return Content(HttpStatusCode.Conflict, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
            }
            catch (Exception ex)
            {
                _logger.Error("Error found for employeeCode: " + value.EmployeeCode + " in Get for EmployeeController.\nException:" + ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionReasons.Generic));
            }
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(string code)
        {
            try
            {
                var isDeleted = await _employeeService.DeleteEmployeeAsync(code);                
                return Ok(isDeleted);

            }
            catch (EmployeeException ex)
            {
                return Content(HttpStatusCode.NotFound, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
            }
            catch(Exception ex)
            {
                _logger.Error("Error found for " + code + " in Delete for EmployeeController.\nException:" + ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionReasons.Generic));
            }
             
        }
    }
}
