using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.CSharp;
using NBomber.Plugins.Network.Ping;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.UnitTests.Fakes;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

//new HttpJson().Run_Places();
//new HttpJson().Run_Users();
new HttpJson().Run_PlacesAmenities();
Console.ReadKey();

public class HttpJson
{
    public void Run_Places()
    {
        using var httpClient = new HttpClient();
        var jwt = string.Empty;
        var scenario = Scenario.Create("http_scenario", async context =>
        {
            var model = new PlaceFake().Generate();
            var json = JsonSerializer.Serialize(model);

            HttpRequestMessage request = CreatePostRequest(jwt, json, "places");

            var response = await Http.Send(httpClient, request);

            return response;
        })
        // irá obter o token jwt    
        .WithInit(async context =>
        {
            var viewModel = await RealizeLogin();
            jwt = viewModel.Token;
        })
        .WithoutWarmUp() //aquecimento da api
        // acada 1 segundo ira entrar 5 novos usuarios durante 30s
        .WithLoadSimulations(Simulation.Inject(rate: 5, interval: TimeSpan.FromSeconds(1),
                                               during: TimeSpan.FromSeconds(5)));

        NBomberRegisterScenarios(scenario);
    }

    public void Run_Users()
    {
        using var httpClient = new HttpClient();
        var jwt = string.Empty;
        var scenario = Scenario.Create("http_scenario", async context =>
        {
            var model = new UserFake().Generate();

            var json = JsonSerializer.Serialize(model);

            HttpRequestMessage request = CreatePostRequest(jwt, json, "users");

            var response = await Http.Send(httpClient, request);

            return response;
        })
        .WithInit(async context =>
        {
            var viewModel = await RealizeLogin();
            jwt = viewModel.Token;
        })
        .WithoutWarmUp() //aquecimento da api
        // acada 1 segundo ira entrar 5 novos usuarios durante 5s
        .WithLoadSimulations(Simulation.Inject(rate: 1, interval: TimeSpan.FromSeconds(1),
                                               during: TimeSpan.FromSeconds(5)));

        NBomberRegisterScenarios(scenario);
    }

    public void Run_PlacesAmenities()
    {
        using var httpClient = new HttpClient();
        var jwt = string.Empty;
        var scenario = Scenario.Create("http_scenario", async context =>
        {
            var model = new AmenityFake().Generate();
            var json = JsonSerializer.Serialize(model);

            HttpRequestMessage request = CreatePostRequest(jwt, json, $"places/{new Random().Next(1, 300)}/amenities");

            var response = await Http.Send(httpClient, request);

            return response;
        })
        // irá obter o token jwt    
        .WithInit(async context =>
        {
            var viewModel = await RealizeLogin();
            jwt = viewModel.Token;
        })
        .WithoutWarmUp() //aquecimento da api
        // acada 1 segundo ira entrar 5 novos usuarios durante 30s
        .WithLoadSimulations(Simulation.Inject(rate: 5, interval: TimeSpan.FromSeconds(1),
                                               during: TimeSpan.FromSeconds(5)));

        NBomberRegisterScenarios(scenario);
    }

    private static HttpRequestMessage CreatePostRequest(string jwt, string json, string endPoint)
    {
        HttpRequestMessage request = Http.CreateRequest("POST", $"https://localhost:7246/api/{endPoint}")
                                         .WithHeader("Authorization", $"Bearer {jwt}")
                                         .WithJsonBody(json);

        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        return request;
    }

    private async Task<LoginViewModel> RealizeLogin()
    {
        using var client = new HttpClient();
        await Task.Delay(TimeSpan.FromSeconds(5));

        var result = await client.PutAsJsonAsync("https://localhost:7246/api/users/login",
            new LoginInputModel
            {
                Email = "filipeadmin@teste.com",
                Password = "123456"
            });

        var viewModel = await result.Content.ReadFromJsonAsync<ResultViewModel<LoginViewModel>>();

        return viewModel!.Data!;
    }

    private static void NBomberRegisterScenarios(ScenarioProps scenario)
    {
        NBomberRunner.RegisterScenarios(scenario)
                     .WithWorkerPlugins(new PingPlugin(PingPluginConfig.CreateDefault("nbomber.com")),
                                        new HttpMetricsPlugin(new[] { HttpVersion.Version1 })).Run();
    }

}