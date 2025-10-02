using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Domain.Entities;

public class Address : IEquatable<Address>
{

    private const int MinLength = 1;
    private const int MaxStreetLength = 200;
    private const int MaxCityLength = 100;
    private const int MaxZipCodeLength = 10;
    private const int MaxCountryLength = 50;

    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
    public string Country { get; private set; }
    public AddressType Type { get; private set; }

    private Address() { } // For EF Core

    public Address(string street, string city, string state, string zipCode, string country, AddressType type)
    {
        Street = street.Trim();
        City = city.Trim();
        State = state.Trim();
        ZipCode = zipCode.Trim();
        Country = country.Trim();
        Type = type;
    }

    public static Result<Address> Create(string street, string city, string state, string zipCode, string country, AddressType type)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.MissingStreet, "Street is required."));
        if (string.IsNullOrWhiteSpace(city))
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.MissingCity, "City is required."));
        if (string.IsNullOrWhiteSpace(state))
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.MissingState, "State is required"));
        if (string.IsNullOrWhiteSpace(zipCode))
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.MissingZipcode, "Zipcode is required"));
        if (string.IsNullOrWhiteSpace(country))
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.MissingCountry, "Country is required"));
        if (street.Trim().Length < MinLength || street.Trim().Length > MaxStreetLength)
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.StreetLengthViolation, $"Street length must be between {MinLength} and {MaxStreetLength} characters"));
        if (city.Trim().Length < MinLength || city.Trim().Length > MaxCityLength)
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.CityLengthViolation, $"City length must be between {MinLength} and {MaxStreetLength} characters"));
        if (state.Trim().Length < MinLength || state.Trim().Length > MaxCityLength)
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.StateLengthViolation, $"State length must be between {MinLength} and {MaxStreetLength} characters"));
        if (zipCode.Trim().Length < MinLength || zipCode.Trim().Length > MaxCityLength)
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.ZipcodeLengthViolation, $"Zipcode length must be between {MinLength} and {MaxStreetLength} characters"));
        if (country.Trim().Length < MinLength || country.Trim().Length > MaxCityLength)
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.CountryLengthViolation, $"Country length must be between {MinLength} and {MaxStreetLength} characters"));
        if (!Enum.IsDefined(typeof(AddressType), type))
            return Result<Address>.Failure(Error.Validation(AddressErrors.Creation.UndefinedType, "Invalid Address Type"));

        var address = new Address(street, city, state, zipCode, country, type);
        return Result<Address>.Success(address);
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
