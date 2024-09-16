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

        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger<CompanyController)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }
        
        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<CompanyDto>> GetAll()
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
        public void Post([FromBody] Company company)
        {
            try
                {
                    if (employee == null)
                        return BadRequest();
        
                    var createdCompany = await _companyService.AddCompany(company);
                    return CreatedAtAction(nameof(Get),
                        new { companyCode = createdCompany.companyCode }, createdCompany);
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while creating a company. " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
        }

        // PUT api/<controller>/5
        [HttpPut"{id:int}"]
        public void Put(int id, [FromBody] Company company)
        {
            try
                {
                    if (id != company.companyCode)
                        return BadRequest("Company code not found");
        
                    var companyToUpdate = await _companyService.GetCompany(id);
        
                    if (companyToUpdate == null)
                        return NotFound($"Company code = {id} not found");
        
                    return await _companyService.UpdateCompany(company);
                }
            catch (Exception ex)
            {
                var msg = "An error occurred while updating company id: + id + ". " + DateTime.Now;
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
                    var companyToDelete = await _companyService.GetCompany(id);
        
                    if (companyToDelete == null)
                    {
                        return NotFound($"Company with Id = {id} not found");
                    }
        
                    return await _companyService.DeleteCompany(id);
                }
            }
            catch (Exception ex)
            {
                var msg = "An error occurred while deleting company id: + id + ". " + DateTime.Now;
                _logger.LogError(ex, msg);
                throw new Exception(msg);
            }    
    }
}
