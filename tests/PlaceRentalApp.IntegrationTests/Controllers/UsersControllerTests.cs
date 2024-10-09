using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaceRentalApp.Infrasctructure.Persistence;
using PlaceRentalApp.UnitTests.Fakes;
using System.Net.Http.Headers;
using System.Text;

namespace PlaceRentalApp.IntegrationTests.Controllers
{
    public class UsersControllerTests : IClassFixture<TestWebFactory<Program>>
    {
        private readonly TestWebFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public UsersControllerTests(TestWebFactory<Program> factory)
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
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        [Fact]
        public async Task AddUser_IsCalledWithValidData_Success()
        {
            // arrange
            var user = new UserFake().Generate();
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // act
            var response = await _httpClient.PostAsync("api/users", content);

            // assert
            response.EnsureSuccessStatusCode();
            var location = response.Headers.GetValues("location").First();
            var id = int.Parse(location.Split('/').Last());

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PlaceRentalDbContext>();
                var exists = context.Users.Any(u => u.Id == id);

                Assert.True(id > 0);
                Assert.True(response.IsSuccessStatusCode);
                Assert.True(exists);
            }
        }

        [Fact]
        public async Task AddUser_IsCalledWithInvalidData_Error()
        {
            // arrange
            var user = new UserFake().Generate();
            user.Password = string.Empty;

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // act
            var response = await _httpClient.PostAsync("api/users", content);

            // assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.True((int)response.StatusCode == StatusCodes.Status400BadRequest);
        }

        // login nao tem como testar em banco de memoria
        //    [Fact]
        //    public async Task Login_ActionLogin_Success()
        //    {
        //        //filipeadmin@teste.com
        //        var login = new LoginInputModel 
        //        {
        //            Email = "filipeadmin@teste.com",
        //            Password = "123456"
        //        };

        //        var json = JsonConvert.SerializeObject(login);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        // act
        //        var response = await _httpClient.PutAsync("api/users/login", content);
        //        response.EnsureSuccessStatusCode();
        //        var location = response.Headers.GetValues("location").First();

        //        // assert
        //        Assert.True(response.IsSuccessStatusCode);
        //    }


    }
}
