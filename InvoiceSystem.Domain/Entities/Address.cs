using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
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

        var errors = new List<Error>();

        string trimmedStreet = street?.Trim() ?? string.Empty;
        string trimmedCity = city?.Trim() ?? string.Empty;
        string trimmedState = state?.Trim() ?? string.Empty;
        string trimmedZipcode = zipCode?.Trim() ?? string.Empty;
        string trimmedCountry = country?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(trimmedStreet))
            errors.Add(Error.Validation(AddressErrors.Creation.MissingStreet, "Street is required."));
        if (string.IsNullOrWhiteSpace(trimmedCity))
            errors.Add(Error.Validation(AddressErrors.Creation.MissingCity, "City is required."));
        if (string.IsNullOrWhiteSpace(trimmedState))
            errors.Add(Error.Validation(AddressErrors.Creation.MissingState, "State is required"));
        if (string.IsNullOrWhiteSpace(trimmedZipcode))
            errors.Add(Error.Validation(AddressErrors.Creation.MissingZipcode, "Zipcode is required"));
        if (string.IsNullOrWhiteSpace(trimmedCountry))
            errors.Add(Error.Validation(AddressErrors.Creation.MissingCountry, "Country is required"));
        if (trimmedStreet.Length > 0 && (trimmedStreet.Length < MinLength || trimmedStreet.Length > MaxStreetLength))
            errors.Add(Error.Validation(AddressErrors.Creation.StreetLengthViolation, $"Street length must be between {MinLength} and {MaxStreetLength} characters"));
        if (trimmedCity.Length > 0 && (trimmedCity.Length < MinLength || trimmedCity.Length > MaxCityLength))
            errors.Add(Error.Validation(AddressErrors.Creation.CityLengthViolation, $"City length must be between {MinLength} and {MaxStreetLength} characters"));
        if (trimmedState.Length > 0 && (trimmedState.Length < MinLength || trimmedState.Length > MaxCityLength))
            errors.Add(Error.Validation(AddressErrors.Creation.StateLengthViolation, $"State length must be between {MinLength} and {MaxStreetLength} characters"));
        if (trimmedZipcode.Length > 0 && (trimmedZipcode.Length < MinLength || trimmedZipcode.Length > MaxCityLength))
            errors.Add(Error.Validation(AddressErrors.Creation.ZipcodeLengthViolation, $"Zipcode length must be between {MinLength} and {MaxZipCodeLength} characters"));
        if (trimmedCountry.Length > 0 && (trimmedCountry.Length < MinLength || trimmedCountry.Length > MaxCityLength))
            errors.Add(Error.Validation(AddressErrors.Creation.CountryLengthViolation, $"Country length must be between {MinLength} and {MaxCountryLength} characters"));
        if (!Enum.IsDefined(typeof(AddressType), type))
            errors.Add(Error.Validation(AddressErrors.Creation.UndefinedType, "Invalid Address Type"));

        if (errors.Any())
            return Result<Address>.Failure(errors);

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
}
