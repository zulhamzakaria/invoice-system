using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class WorkflowstepService : IWorkflowstepService
{
    private readonly IWorkflowStepRepository _workflowStepRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public WorkflowstepService(IWorkflowStepRepository workflowStepRepository, IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
        _workflowStepRepository = workflowStepRepository;
    }
    public async Task<Result<WorkflowstepsDetailsDTO>> CreateWorkflowstepAsync(WorkflowstepsCreationDTO dto)
    {
        var invoice  = await _invoiceRepository.GetByIdAsync(dto.InvoiceId);
        if (invoice == null)
        {
            var errors = new List<Error> {}
        }
    }
}
