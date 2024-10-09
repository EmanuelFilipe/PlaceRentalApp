using Bogus;
using PlaceRentalApp.Core.Entities;

namespace PlaceRentalApp.UnitTests.Fakes
{
    public class PlaceFake : Faker<Place>
    {
        public PlaceFake(int id = 0)
        {
            CustomInstantiator(f => new Place(
                f.Random.Word(),
                f.Random.Word(),
                f.Random.Number(1, 200),
                new AddressFake().Generate(),
                f.Random.Number(1,5),
                true,
                f.Random.Number(1, 3)) //f.Random.Number(1,990))
            );

            RuleFor(place => place.Address, f => new AddressFake().Generate());
            RuleFor(place => place.User, f => new UserFake().Generate()); 
            RuleFor(place => place.Amenities, f => new AmenityFake().Generate(5));//gera 5

            if (id > 0)
                RuleFor(p => p.Id, faker => id);
        }
    }
}
