using Bogus;
using PlaceRentalApp.Core.Entities;

namespace PlaceRentalApp.UnitTests.Fakes
{
    public class AmenityFake : Faker<PlaceAmenity>
    {
        public AmenityFake()
        {
            CustomInstantiator(f => new PlaceAmenity(f.Random.Word(), 
                                                     f.Random.Number(1, 300))
            );
        }
    }
}
