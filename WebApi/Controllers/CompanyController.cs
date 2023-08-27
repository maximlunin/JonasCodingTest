﻿using System;
using System.Collections.Generic;
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

		public CompanyController(ICompanyService companyService, IMapper mapper, ILogger logger)
		{
			_companyService = companyService;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<IEnumerable<CompanyDto>> GetAllAsync()
		{
			var items = await _companyService.GetAllAsync();
			return _mapper.Map<IEnumerable<CompanyDto>>(items);
		}

		public async Task<IHttpActionResult> GetAsync(string companyCode)
		{
			var item = await _companyService.GetByCodeAsync(companyCode);
			if (item is null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<CompanyDto>(item));
		}

		public async Task<IHttpActionResult> PostAsync([FromBody] CompanyDto companyDto)
		{
			var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
			var res = await _companyService.SaveAsync(companyInfo);
			return SaveResultToActionResult(res, companyInfo);
		}

		public async Task<IHttpActionResult> PutAsync(string companyCode, [FromBody] CompanyDto companyDto)
		{
			var oldItem = await _companyService.GetByCodeAsync(companyCode);
			if (oldItem is null)
			{
				return NotFound();
			}

			var newItem = _mapper.Map<CompanyInfo>(companyDto);
			return SaveResultToActionResult(await _companyService.SaveAsync(newItem, oldItem), newItem);
		}

		public async Task<IHttpActionResult> DeleteAsync(string companyCode)
		{
			if (await _companyService.DeleteAsync(companyCode))
			{
				return Ok();
			}

			// This operation shouldn't fail.
			_logger
				.ForContext(nameof(companyCode), companyCode)
				.Fatal("Unknown error attempting to delete company.");
			return InternalServerError();
		}

		private IHttpActionResult SaveResultToActionResult(CompanySaveResult result, CompanyInfo companyInfo)
		{
			switch (result)
			{
				case CompanySaveResult.Success:
					return Created(
						$"/api/company/{companyInfo?.CompanyCode}",
						_mapper.Map<CompanyDto>(companyInfo)
					);

				case CompanySaveResult.DuplicateKey:
					return BadRequest("Company code already exists.");

				case CompanySaveResult.MissingCode:
					return BadRequest("Missing company code.");

				case CompanySaveResult.InvalidValue:
					return BadRequest("Cannot specify value for site ID.");

				case CompanySaveResult.CannotChangeCode:
					return BadRequest("Cannot change company code.");

				default:
					_logger
						.ForContext(nameof(companyInfo), companyInfo, true)
						.ForContext(nameof(result), result)
						.Error("Unknown result.");
					throw new NotSupportedException();
			}
		}
	}
}