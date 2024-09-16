using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using WebApi.Models;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }
        
        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<EmployeeDto>> GetAll()
        {
            try
                {
                    var items = _employeeService.GetAllCompanies();
                    return _mapper.Map<IEnumerable<EmployeeDto>>(items);
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while listing all companies. " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // GET api/<controller>/5
        [HttpGet("{employeeCode:string}")]
        public EmployeeDto Get(string employeeCode)
        {
            try
                {
                    var item = _employeeService.GetEmployeeByCode(employeeCode);
                    return _mapper.Map<EmployeeDto>(item);
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while retrieving " + employeeCode + " employee. " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] Employee employee)
        {
            try
                {
                    if (employee == null)
                        return BadRequest();
        
                    var createdEmployee = await _employeeService.AddEmployee(employee);
        
                    return CreatedAtAction(nameof(Get),
                        new { employeeCode = createdEmployee.employeeCode }, createdEmployee);
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while creating a employee. " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // PUT api/<controller>/5
        [HttpPut"{id:int}"]
        public void Put(int id, [FromBody] Employee employee)
        {
            try
                {
                    if (id != employee.employeeCode)
                        return BadRequest("Employee code not found");
        
                    var employeeToUpdate = await _employeeService.GetEmployee(id);
        
                    if (employeeToUpdate == null)
                        return NotFound($"Employee code = {id} not found");
        
                    return await _employeeService.UpdateEmployee(employee);
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while updating employee id: + id + ". " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            try
                {
                    var employeeToDelete = await _employeeService.GetEmployee(id);
        
                    if (employeeToDelete == null)
                    {
                        return NotFound($"Employee with Id = {id} not found");
                    }
        
                    return await _employeeService.DeleteEmployee(id);
                }
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while deleting employee id: + id + ". " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
    }
}
