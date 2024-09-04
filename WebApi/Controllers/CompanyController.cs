using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using Serilog;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = SerilogLogger._logger; ;
        }
        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            try
            {
                var items = await _companyService.GetAllCompaniesAsync();
                return _mapper.Map<IEnumerable<CompanyDto>>(items);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        // GET api/<controller>/5
        public async Task<CompanyDto> Get(string companyCode)
        {
            if (String.IsNullOrEmpty(companyCode))
            {
                throw new Exception(HttpStatusCode.BadRequest.ToString());
            }
            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(companyCode);
                return _mapper.Map<CompanyDto>(item);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new Exception(HttpStatusCode.NotFound.ToString());
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var company = _mapper.Map<CompanyInfo>(companyDto);

                await _companyService.SaveCompanyAsync(company);

                return Ok("Company Inserted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerError();
            }
        }
        [HttpPut]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(string companyId, [FromBody] CompanyDto companyDto)
        {
            if (String.IsNullOrEmpty(companyId))
            {
                return BadRequest("company was null or empty");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var company = _mapper.Map<CompanyInfo>(companyDto);

                await _companyService.UpdateCompanyAsync(companyId, company);

                return Ok("Company details updated");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerError();
            }

        }
        [HttpDelete]
        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(string companyId)
        {
            if (String.IsNullOrEmpty(companyId))
            {
                return BadRequest("Company Id was not provided");
            }
            try
            {
                await _companyService.DeleteCompanyAsync(companyId);

                return Ok("company deleted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerError();
            }
        }
    }
}