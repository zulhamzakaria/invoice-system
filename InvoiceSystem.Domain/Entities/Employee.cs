namespace InvoiceSystem.Domain.Entities;

public abstract class Employee
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Email { get; private set; }

    protected Employee() { } // For EF Core

    protected Employee(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
