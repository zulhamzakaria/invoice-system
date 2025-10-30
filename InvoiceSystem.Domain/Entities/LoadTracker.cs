namespace InvoiceSystem.Domain.Entities;

public class LoadTracker
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ApproverId { get; private set; }
    public Employee Approver { get; private set; }
    public int ActiveAssignments { get; private set; }
    public DateTimeOffset LastAssignedAt { get; private set; }

    public LoadTracker()
    {
        //for EF Core
    }

    public static LoadTracker Create(Guid approverId)
    {
        return new LoadTracker{ApproverId = approverId, ActiveAssignments = 0, LastAssignedAt = DateTimeOffset.MinValue};
    }

    public void MarkAssigned()
    {
        ActiveAssignments += 1;
        LastAssignedAt = DateTimeOffset.UtcNow;
    }

    public void MarkCompleted()
    {
        if(ActiveAssignments > 0)
        {
            ActiveAssignments--;
        }
    }
}
