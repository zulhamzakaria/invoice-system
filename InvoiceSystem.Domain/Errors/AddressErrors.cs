namespace InvoiceSystem.Domain.Errors;

public static class AddressErrors
{
    public static class Creation
    {
        public const string MissingStreet = "ADD.ADDRESS_STREET_REQUIRED";
        public const string MissingCity = "ADD.ADDRESS_CITY_REQUIRED";
        public const string MissingState = "ADD.ADDRESS_STATE_REQUIRED";
        public const string MissingZipcode = "ADD.ADDRESS_ZIPCODE_REQUIRED";
        public const string MissingCountry = "ADD.ADDRESS_COUNTRY_REQUIRED";
        public const string UndefinedType = "ADD.ADDRESS_TPYE_UNDEFINED";
        public const string StreetLengthViolation = "ADD.ADDRESS_STREET_LENGTH_INVALID";
        public const string CityLengthViolation = "ADD.ADDRESS_CITY_LENGTH_INVALID";
        public const string CountryLengthViolation = "ADD.ADDRESS_COUNTRY_LENGTH_INVALID";
        public const string StateLengthViolation = "ADD.ADDRESS_STATE_LENGTH_INVALID";
        public const string ZipcodeLengthViolation = "ADD.ADDRESS_ZIPCODE_LENGTH_INVALID";
    }
}
