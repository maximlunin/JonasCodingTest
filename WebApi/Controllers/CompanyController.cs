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
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            _logger.LogInformation("Handling GET request to retrieve all companies");
            try
            {
                var companies = await _companyService.GetAllAsync();

                if (companies == null || !companies.Any())
                {
                    _logger.LogWarning("No companies found.");
                    return Enumerable.Empty<CompanyDto>();
                }
                _logger.LogInformation("Successfully retrieved companies.");
                return _mapper.Map<IEnumerable<CompanyDto>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving companies.");
                return Enumerable.Empty<CompanyDto>();
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{companyCode}", Name = "GetCompanyByCode")]
        public async Task<CompanyDto> Get(string companyCode)
        {
            _logger.LogInformation($"Handling GET request to retrieve company with code: {companyCode}");
            try
            {
                var company = await _companyService.GetByCodeAsync(companyCode);

                if (company == null)
                {
                    _logger.LogWarning($"No company found with code: {companyCode}");
                    return null;
                }

                _logger.LogInformation($"Successfully retrieved company with code: {companyCode}");
                return _mapper.Map<CompanyDto>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the company with code: {companyCode}");
                return null;
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            _logger.LogInformation("Handling POST request to add a new company.");

            if (companyDto == null || string.IsNullOrWhiteSpace(companyDto.CompanyName))
            {
                _logger.LogWarning("Invalid company data received.");
                return BadRequest("Company data is invalid.");
            }
            try
            {
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var success = await _companyService.AddAsync(companyInfo);

                if (success)
                {
                    _logger.LogInformation($"Company '{companyDto.CompanyName}' added successfully.");
                    return CreatedAtRoute("GetCompanyByCode", new { companyCode = companyInfo.CompanyCode }, _mapper.Map<CompanyDto>(companyInfo));
                }
                else
                {
                    _logger.LogError($"Failed to add company '{companyDto.CompanyName}'.");
                    return InternalServerError(new Exception("Failed to add company."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding company '{companyDto.CompanyName}'.");
                return InternalServerError(ex);
            }

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] CompanyDto companyDto)
        {
            _logger.LogInformation($"Handling PUT request to update company with id: {id}");

            if (companyDto == null || string.IsNullOrWhiteSpace(companyDto.CompanyName))
            {
                _logger.LogWarning("Invalid company data received for update.");
                return BadRequest("Invalid company data.");
            }

            try
            {
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var success = await _companyService.UpdateAsync(id, companyInfo);
                if (success)
                {
                    _logger.LogInformation($"Successfully updated company with id: {id}");
                    return Ok(_mapper.Map<CompanyDto>(companyInfo));
                }
                else
                {
                    _logger.LogWarning($"No company found with id: {id} to update.");
                    return Content(HttpStatusCode.NotFound, $"No company found with id: {id} to update.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the company with id: {id}");
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            _logger.LogInformation($"Handling DELETE request for company with id: {id}");

            try
            {
                var success = await _companyService.DeleteAsync(id);

                if (success)
                {
                    _logger.LogInformation($"Successfully deleted company with id: {id}");
                    return Ok("Company deleted successfully.");
                }
                else
                {
                    _logger.LogWarning($"No company found with id: {id} to delete.");
                    return Content(HttpStatusCode.NotFound, $"No company found with id: {id} to delete.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the company with id: {id}");
                return InternalServerError(ex);
            }
        }
    }
}