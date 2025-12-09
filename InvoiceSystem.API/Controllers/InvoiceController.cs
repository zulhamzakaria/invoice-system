using InvoiceSystem.API.Models;
using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Application.DTOs.InvoiceOrchestrator;
using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
            return Ok(results.Value);
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

        [HttpGet("{invoiceId:guid}/workflow-history")]
        public async Task<IActionResult> GetWorkflowHistory(Guid invoiceId)
        {
            var results = await _invoiceService.GetInvoiceHistoryAsync(invoiceId);
            if (results.IsFailure)
            {
                return BadRequest(results.Errors);
            }
            return Ok(results.Value);
        }

        [HttpGet("{employeeId:guid}/approver-tasks")]
        public async Task<IActionResult> GetApproverTasks(Guid employeeId)
        {
            var results = await _invoiceService.GetApproverTasksAsync(employeeId);
            return results.IsFailure ? BadRequest(results.Errors) 
                : Ok(results.Value); 
        }

        [HttpGet("approvers-tasks")]
        public IActionResult GetAllApproversTasks()
        {
            var results = _invoiceService.GetAllApproversTasks();
            return results.IsFailure ? BadRequest(results.Errors)
                : Ok(results.Value);
        }

        [HttpGet("{employeeId:guid}/clerk-tasks")]
        public async Task<IActionResult> GetClerkTasks(Guid employeeId)
        {
            var results = await _invoiceService.GetClerkTasksAsync(employeeId);
            return results.IsFailure ? BadRequest(results.Errors) 
                : Ok(results.Value);
        }

        [HttpGet("{invoiceId:guid}/items")]
        public async Task<IActionResult> GetAllInvoiceItems(Guid invoiceId)
        {
            var results = await _invoiceService.GetAllInvoiceItemsAsync(invoiceId);
            if (results is null)
            {
                return NotFound(ErrorCodes.NotFound<IInvoiceService>());
            }
            if (results.IsFailure)
            {
                return BadRequest(results.Errors);
            }
            return Ok(results.Value);
        }

        [HttpPost("{invoiceId:guid}/items")]
        public async Task<IActionResult> AddInvoiceItem(Guid invoiceId, [FromBody] InvoiceItemCreationDTO dto)
        {
            var result = await _invoiceService.CreateInvoiceItemAsync(invoiceId, dto);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            //Created at action;
            return CreatedAtAction(nameof(GetAllInvoiceItems), new {invoiceId}, result.Value);
        }

        [HttpPost("{invoiceId:guid}/items/delete")]
        public async Task<IActionResult> DeleteInvoiceItem(Guid invoiceId, [FromBody] InvoiceItemDeleteDTO dto)
        {
            var result = await _invoiceService.DeleteInvoiceItemAsync(invoiceId, dto.ItemId, dto.EmployeeId);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPost("{invoiceId:guid}/items/batch-delete")]
        public async Task<IActionResult> DeleteInvoiceItemsByBatch(Guid invoiceId, InvoiceItemsDeleteDTO dto)
        {
            var result = await _invoiceService.DeleteInvoiceItemsAsync(invoiceId, dto.ItemIds, dto.EmployeeId);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPost("{invoiceId:guid}/submit")]
        [SwaggerOperation(Summary = "Note: Reason is for Voiding, Rejecting Invoice")]
        public async Task<IActionResult> SubmitInvoice(Guid invoiceId, [FromBody] WorkflowstepsActionDTO dto)
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
        public async Task<IActionResult> RejectInvoice(Guid invoiceId, [FromBody] WorkflowstepsActionDTO dto)
        {
            var result = await _invoiceOrchestratorService.RejectInvoiceAsync(invoiceId, dto);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpPost("{invoiceId:guid}/void")]
        public async Task<IActionResult> VoidInvoice(Guid invoiceId, [FromBody] WorkflowstepsActionDTO dto)
        {
            var result = await _invoiceOrchestratorService.VoidInvoiceAsync(invoiceId, dto);
            if (result.IsFailure)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }
    }
}
