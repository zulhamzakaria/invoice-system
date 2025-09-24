using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Domain.Common;

public abstract class AuditableEntity : IAuditable
{
    public DateTime CreatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedById { get; set; }
}