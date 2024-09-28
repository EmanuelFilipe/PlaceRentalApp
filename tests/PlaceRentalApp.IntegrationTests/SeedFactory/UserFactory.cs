using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.UnitTests.Fakes;

namespace PlaceRentalApp.IntegrationTests.SeedFactory
{
    public class UserFactory
    {
        public static User GetUser() => new UserFake().Generate();
    }
}
