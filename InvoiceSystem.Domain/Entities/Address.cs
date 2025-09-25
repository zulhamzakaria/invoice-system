namespace InvoiceSystem.Domain.Entities;

public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
    public string Country { get; private set; }
    public AddressType Type { get; set; }

    private Address() { } // For EF Core

    public Address(string street, string city, string state, string zipCode, string country, AddressType type)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Type = type;
    }
}

public enum AddressType
{
    HQ,
    Billing,
    Shipping
}