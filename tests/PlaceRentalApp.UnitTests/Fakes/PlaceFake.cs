using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceRentalApp.UnitTests.Fakes
{
    public class AddressFake : Faker<Address>
    {
        public AddressFake()
        {
            //string[] districts = GetDistricts();

            CustomInstantiator(faker => new Address(faker.Address.StreetName(),
                faker.Address.BuildingNumber(),
                faker.Address.ZipCode(),
                faker.PickRandom(GetDistricts()),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country())
            );
        }

        private static string[] GetDistricts()
        {
            var districts = new[]
            {
                "Manhattan",        // New York, NY
                "Brooklyn",         // New York, NY
                "Downtown",         // Los Angeles, CA
                "Hollywood",        // Los Angeles, CA
                "Mission District", // San Francisco, CA
                "SoHo",             // New York, NY
                "The Loop",         // Chicago, IL
                "South Beach",      // Miami, FL
                "Georgetown",       // Washington, D.C.
                "Capitol Hill",     // Seattle, WA
                "French Quarter",   // New Orleans, LA
                "River North",      // Chicago, IL
                "Wynwood",          // Miami, FL
                "Silver Lake",      // Los Angeles, CA
                "Beacon Hill",      // Boston, MA
                "Nob Hill",         // San Francisco, CA
                "Old Town",         // San Diego, CA
                "Uptown",           // Dallas, TX
                "East Village",     // New York, NY
                "Belltown",         // Seattle, WA
                "Magnolia",         // Seattle, WA
                "Tribeca",          // New York, NY
                "Lakeview",         // Chicago, IL
                "Midtown",          // Atlanta, GA
                "Fremont",          // Seattle, WA
                "Back Bay",         // Boston, MA
                "Chinatown",        // San Francisco, CA
                "Little Havana",    // Miami, FL
            };
            return districts;
        }
    }

    public class UserFake: Faker<User>
    {
        public UserFake()
        {
            var roles = new[]
            {
                "admin",
                "client",
                "freelancer"
            };

            CustomInstantiator(faker => new User(
                faker.Name.FullName(),
                faker.Internet.Email(),
                faker.Date.Recent(365),
                faker.Internet.Password(),
                faker.PickRandom(roles))
            );
        }
    }

    public class AmenityFake : Faker<PlaceAmenity>
    {
        public AmenityFake()
        {
            CustomInstantiator(f => new PlaceAmenity(f.Random.Word(), 
                                                     f.Random.Number(1, 999))
            );
        }
    }

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
                f.Random.Number(1,990))
            );

            RuleFor(place => place.Address, f => new AddressFake().Generate());
            RuleFor(place => place.User, f => new UserFake().Generate()); 
            RuleFor(place => place.Amenities, f => new AmenityFake().Generate(5));//gera 5

            if (id > 0)
                RuleFor(p => p.Id, faker => id);
        }
    }
}
