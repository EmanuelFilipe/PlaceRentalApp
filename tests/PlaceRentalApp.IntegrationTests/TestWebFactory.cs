using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaceRentalApp.Infrasctructure.Persistence;
using PlaceRentalApp.IntegrationTests.SeedFactory;

namespace PlaceRentalApp.IntegrationTests
{
    // Program -> from Progam.cs in .API 
    public class TestWebFactory<T> : WebApplicationFactory<Program>
    {
        public int PlaceId;
        public int UserId;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //configurando banco de dados em memória
                var dbContextDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<PlaceRentalDbContext>));

                if (dbContextDescriptor is not null)
                    services.Remove(dbContextDescriptor);

                services.AddDbContext<PlaceRentalDbContext>(o =>
                    o.UseInMemoryDatabase("PlaceRentalDb"));

                var provider = services.BuildServiceProvider();

                using (var scope = provider.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<PlaceRentalDbContext>())
                    {
                        context.Database.EnsureCreated();

                        var user = UserFactory.GetUser();
                        context.Users.Add(user);
                        context.SaveChanges();
                        UserId = user.Id;


                        var place = PlaceFactory.GetPlace(user.Id);
                        context.Places.Add(place);
                        context.SaveChanges();
                        PlaceId = place.Id;

                        var amenities = PlaceFactory.GetAmenities(place.Id);
                        context.PlaceAmenities.AddRange(amenities);
                        context.SaveChanges();
                    }
                }
                //var repositoryDescriptor = services.SingleOrDefault(d => 
                //    d.ServiceType == typeof(IPlaceRepository));

                //if (repositoryDescriptor is not null)

            });

            base.ConfigureWebHost(builder);
        }
    }
}
