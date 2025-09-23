namespace InvoiceSystem.Domain.Entities;

public class Company
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string RegistrationNumber { get; private set; }
    public List<Address> Addresses { get; private set; } = new();

    private Company() { } // For EF Core

    public Company(string name, string registrationNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Company name cannot be empty.");

        Name = name;
        RegistrationNumber = registrationNumber;
    }

    public void AddAddress(Address address)
    {
        Addresses.Add(address);
    }
}
