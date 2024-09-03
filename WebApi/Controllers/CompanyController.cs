using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
//using Microsoft.Extensions.Logging;
//using Ninject.Extensions.Logging;
using WebApi.Models;
using Serilog;
using BusinessLayer.Model.Exceptions;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper,ILogger logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;           
        }

        // GET api/<controller>
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var items = await _companyService.GetAllCompaniesAsync();
                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(items));
            }
            catch(Exception ex)
            {
                _logger.Error("CompanyController: GetAll(), Exception: " + ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionReasons.Generic));
            }
        }

        // GET api/<controller>/{code}
        public async Task<IHttpActionResult> Get(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Company code was null or empty");
            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(code);              
                return Ok(item);
            }
            catch(CompanyException ex)
            {
                return Content(HttpStatusCode.NotFound, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
            }
            catch(Exception ex)
            {
                _logger.Error("CompanyController: Get, Exception: " + ex.Message);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionReasons.Generic));
                
            }

        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post([FromBody]CompanyDto value)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = _mapper.Map<CompanyInfo>(value);

                await _companyService.SaveCompanyAsync(company, true);

                return Created(company.CompanyCode, company);

            }catch(CompanyException ex)
            {
                return Content(HttpStatusCode.InternalServerError, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        // PUT api/<controller>/{companyCode}
        public async Task<IHttpActionResult> Put(string code, [FromBody]CompanyDto value)
        {
            if (code == null)
                BadRequest("Please provide a company code to update");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);              
            }

            // changing the company code for existing company would be not possible due to database limitaions
            if (!code.Equals(value.CompanyCode, StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("Changing the company code is not supported, please ensure you have same company code in uri and body");
            

            try
            {
                var company = _mapper.Map<CompanyInfo>(value);
                
                await _companyService.SaveCompanyAsync(company, false);

                return Content(HttpStatusCode.OK, code);

            }catch(CompanyException ex)
            {
                if(ex.ExceptionReason is ExceptionReason.CompanyDoesntExist)
                {
                    return Content(HttpStatusCode.NotFound, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));

                }
                else if(ex.ExceptionReason is ExceptionReason.SiteIdNoCorrectForCompany)
                {
                    return Content(HttpStatusCode.Conflict, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, ExceptionReasonDescription.GetDescription(ex.ExceptionReason));
                }

            }catch(Exception ex)
            {
                //_logger.Error("CompanyController: Put, Exception: " + ex.Message);
                throw ex;
            }
           
        }

        // DELETE api/<controller>/5
        public async Task<IHttpActionResult> Delete(string code, bool withEmployees = false)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("CompanyCode is required to delete a company");

            try
            {
                var isDeleted = await _companyService.DeleteCompanyAsync(code, withEmployees);

                return Content(HttpStatusCode.OK, "Successfully deleted");
            
            }
            catch(CompanyException ex)
            {
                var errorDescription = ExceptionReasonDescription.GetDescription(ex.ExceptionReason);

                return Content(HttpStatusCode.Conflict, errorDescription);

            }catch(Exception ex)
            {
                _logger.Error("Error in CompanyController: Delete, Exception: " + ex.Message);
                return Content(HttpStatusCode.InternalServerError, ExceptionReasons.Generic);
            }
        }
    }
}