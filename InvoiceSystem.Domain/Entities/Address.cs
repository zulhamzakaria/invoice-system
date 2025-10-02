using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Domain.Entities;

public class Address : IEquatable<Address>
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
    public string Country { get; private set; }
    public AddressType Type { get; private set; }

    private Address() { } // For EF Core

    public Address(string street, string city, string state, string zipCode, string country, AddressType type)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        Type = type;
    }

    public static Result<Address> Create (string street, string city, string state, string zipCode, string country, AddressType type)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Result<Address>.Failure(Error.Validation("", ""));
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Address);
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Street == other.Street
                && City == other.City
                && State == other.State
                && ZipCode == other.ZipCode
                && Country == other.Country
                && Type == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode, Country, Type);
    }

public enum AddressType
{
    HQ,
    Billing,
    Shipping
}
}
