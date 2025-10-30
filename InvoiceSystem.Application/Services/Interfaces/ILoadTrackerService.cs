using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface ILoadTrackerService
{
    Task<Result<Employee>> GetNextApproverAsync(decimal invoiceAmount);
    Task<Result> RecordAssignmentAsync(Guid employeeId);
}
