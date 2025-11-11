using InvoiceSystem.API.Models;
using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCompanyDetails(Guid id)
        {
            var result = await _companyService.GetCompanyByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(new ErrorResponse(result.Errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _companyService.GetAllCompaniesAsync();
            if (result.IsFailure)
            {
                return BadRequest(new ErrorResponse(result.Errors));
            }
            return Ok(result.Value);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CompanyDetailsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreationDTO companyCreationDTO)
        {
            var result = await _companyService.CreateCompanyAsync(companyCreationDTO);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCompanyDetails), new {id=result.Value.Id},result.Value);
            }
            return BadRequest(new ErrorResponse(result.Errors));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CompanyDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyUpdateDTO dto)
        {
            var result = await _companyService.UpdateCompanyAsync(id, dto);
            if (result.IsSuccess) { return Ok(result.Value); }

            return BadRequest(new ErrorResponse(result.Errors));
        }
    }
}
