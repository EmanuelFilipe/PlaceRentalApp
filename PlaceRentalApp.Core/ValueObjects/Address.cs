namespace PlaceRentalApp.Core.ValueObjects
{
    public record Address(string Street, string Number, string ZipCode, string District,
                         string City, string State, string Country)
    {
        //public string Street { get; private set; } = street;
        //public string Number { get; private set; } = number;
        //public string ZipCode { get; private set; } = zipCode;
        //public string District { get; private set; } = district;
        //public string City { get; private set; } = city;
        //public string State { get; private set; } = state;
        //public string Country { get; private set; } = country;

        public string GetFullAddress() =>
            $"{Street}, {Number}, {ZipCode}, {District}, {City}, {State}, {Country}";
    }
}
