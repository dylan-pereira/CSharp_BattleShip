using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BattleShip.App;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = "http://localhost:5073";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
builder.Services.AddScoped(sp => 
{
    var HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var channel = GrpcChannel.ForAddress("https://localhost:5000", new GrpcChannelOptions { HttpClient = HttpClient});
    return new BattleshipService.BattleshipServiceClient(channel);
});
builder.Services.AddSingleton<GameState>();

builder.Services.AddScoped<IGameService, GameService>();

await builder.Build().RunAsync();
