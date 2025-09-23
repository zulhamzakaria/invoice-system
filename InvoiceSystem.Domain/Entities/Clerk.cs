namespace InvoiceSystem.Domain.Entities;

public class Clerk : Employee
{
    private Clerk() { } // For EF Core
    public Clerk(string name, string email) : base(name, email) { }
}