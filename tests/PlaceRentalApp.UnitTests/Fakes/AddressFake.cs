using Bogus;
using PlaceRentalApp.Core.ValueObjects;

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
}
