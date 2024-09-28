using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.ValueObjects;

namespace PlaceRentalApp.IntegrationTests.SeedFactory
{
    public class PlaceFactory
    {
        public static Place GetPlace(int userId) => new 
            ("title", "description", 100m,
             new Address("street", "123", "123456", "district", "City", "state", "Country"),
             4, true, userId);

        public static List<PlaceAmenity> GetAmenities(int placeId) =>
            [
                new PlaceAmenity("2 Bedrooms", placeId),
                new PlaceAmenity("Hairdryer", placeId),
                new PlaceAmenity("Air Conditioner", placeId)
            ];
    }
}
