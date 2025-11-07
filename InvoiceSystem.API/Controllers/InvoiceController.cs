using InvoiceSystem.API.Models;
using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceOrchestratorService _invoiceOrchestratorService;
        public InvoiceController(IInvoiceService invoiceService, 
                                 IInvoiceOrchestratorService invoiceOrchestratorService)
        {
            _invoiceService = invoiceService;
            _invoiceOrchestratorService = invoiceOrchestratorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var results = await _invoiceService.GetAllInvoicesAsync();
            if (results is null)
            {
                return NotFound(ErrorCodes.NotFound<IInvoiceService>());
            }
            if (results.IsFailure)
            {
                return BadRequest(results.Errors);
            }
            return Ok(results);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetInvoiceDetails(Guid id)
        {
            var result = await _invoiceService.GetInvoiceByIdAsync(id);

            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
                //return BadRequest(new ErrorResponse(result.Errors));
            }
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceCreationDTO dto)
        {
            var result = await _invoiceService.CreateInvoiceAsync(dto);
            if(result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return CreatedAtAction(nameof(GetInvoiceDetails), new {id = result.Value.Id}, result.Value);
        }

    }
}
