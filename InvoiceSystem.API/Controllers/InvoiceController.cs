using InvoiceSystem.API.Models;
using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Application.DTOs.InvoiceOrchestrator;
using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
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
        public async Task<IActionResult> GetAllInvoices()
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
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return CreatedAtAction(nameof(GetInvoiceDetails), new { id = result.Value.Id }, result.Value);
        }

        [HttpPost("{invoiceId}/items")]
        public async Task<IActionResult> AddInvoiceItem(Guid invoiceId, [FromBody] InvoiceItemCreationDTO dto)
        {
            var result = await _invoiceService.CreateInvoiceItemAsync(invoiceId, dto);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            //Created at action
            return Ok(result);
        }

        [HttpPost("{invoiceId:guid}/submit")]
        public async Task<IActionResult> SubmitInvoice(Guid invoiceId, [FromBody] WorkflowstepsCreationDTO dto)
        {
            var result = await _invoiceOrchestratorService.SubmitInvoiceAsync(invoiceId, dto);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPost("{invoiceId:guid}/approve")]
        public async Task<IActionResult> ApproveInvoice(Guid invoiceId, [FromBody] InvoiceApprovalDTO dto)
        {
            var result = await _invoiceOrchestratorService.ApproveInvoiceAsync(invoiceId, dto.approverId);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPost("{invoiceId:guid}/reject")]
        public async Task<IActionResult> RejectInvoice(Guid invoiceId, [FromBody] InvoiceRejectionDTO dto)
        {
            var result = await _invoiceOrchestratorService.RejectInvoiceAsync(invoiceId, dto.employeeId, dto.reason.Trim());
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPost("{invoiceId:guid}/voiding")]
        public async Task<IActionResult> VoidInvoice(Guid invoiceId, [FromBody] InvoiceRejectionDTO dto)
        {
            var result = await _invoiceOrchestratorService.VoidInvoiceAsync(invoiceId, dto.employeeId, dto.reason.Trim());
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }
    }
}
