using Bogus;
using PlaceRentalApp.Core.Entities;

namespace PlaceRentalApp.UnitTests.Fakes
{
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
}
