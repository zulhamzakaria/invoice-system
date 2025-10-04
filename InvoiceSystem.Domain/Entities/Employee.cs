using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Domain.Entities;

public abstract class Employee:AuditableEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Email { get; private set; }

    protected Employee() { } // For EF Core

    protected static Result<Employee> CreateBase(string name, string email)
    {
        var errors = new List<Error>();
        if(string.IsNullOrEmpty(name)) 
            errors.Add(Error.Validation(Empl))
    }

    protected Employee(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
