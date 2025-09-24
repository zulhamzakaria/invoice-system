namespace InvoiceSystem.Domain.Interfaces;

public interface IApprover
{
    bool canApprove(decimal amount);
}
