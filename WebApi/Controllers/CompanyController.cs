using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using WebApi.Models;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;  

        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger<TodoController> logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }
        
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<CompanyDto> GetAll()
        {
            try
                {
                    var items = _companyService.GetAllCompanies();
                    return _mapper.Map<IEnumerable<CompanyDto>>(items);
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while listing all companies. " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{companyCode:string}")]
        public CompanyDto Get(string companyCode)
        {
            try
                {
                    var item = _companyService.GetCompanyByCode(companyCode);
                    return _mapper.Map<CompanyDto>(item);                
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while retrieving " + companyCode + " company. " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Company company)
        {
            try
                {
                    await _companyService.AddCompany(company);
                    return Ok(new { message = "Company successfully created" });
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while creting a company. " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Company company)
        {
            try
                {
                    await _companyService.AddCompany(company);
                    return Ok(new { message = "Company successfully updated" });
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while updating company id: " + id + ". " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
                {
                    await _companyService.DeleteCompany(id);
                    return Ok(new { message = $"Company successfully deleted" });
                }
            catch (Exception ex)
            {
                vvar msg = "An error occurred while deleting company id: " + id + ". " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }
    }
}
