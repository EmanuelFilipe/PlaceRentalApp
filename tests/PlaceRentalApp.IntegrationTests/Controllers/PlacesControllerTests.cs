using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaceRentalApp.Infrasctructure.Persistence;
using PlaceRentalApp.UnitTests.Fakes;

namespace PlaceRentalApp.IntegrationTests.Controllers
{
    // Program -> from Progam.cs in .API 
    public class PlacesControllerTests : IClassFixture<TestWebFactory<Program>>
    {
        private readonly TestWebFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public PlacesControllerTests(TestWebFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "TestScheme", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme");
        }

        [Fact]
        public async Task AddPlace_IsCalledWithValidData_Success()
        {
            // arrange
            var place = new PlaceFake().Generate();
            place.SetCreatedBy(_factory.UserId);

            var json = JsonConvert.SerializeObject(place);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // act
            var response = await _httpClient.PostAsync("api/places", content);

            // assert
            response.EnsureSuccessStatusCode();
            var location = response.Headers.GetValues("location").First();
            var id = int.Parse(location.Split('/').Last());

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PlaceRentalDbContext>();
                //nesse contexto existe algum place com o id retornado?
                var exists = context.Places.Any(p => p.Id == id);

                Assert.True(exists);
            }
        }

        [Fact]
        public async Task AddPlace_IsCalledWithInvalidData_Error()
        {
            // arrange
            var place = new PlaceFake().Generate();
            place.Update(string.Empty, string.Empty, 0);

            var json = JsonConvert.SerializeObject(place);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // act
            var response = await _httpClient.PostAsync("api/places", content);

            // assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.True((int)response.StatusCode == StatusCodes.Status400BadRequest);
        }
    }
}
