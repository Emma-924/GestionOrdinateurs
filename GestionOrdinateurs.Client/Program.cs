using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GestionOrdinateurs.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Session Blazor : sert à mémoriser le token JWT de l'utilisateur connecté
builder.Services.AddBlazoredSessionStorage();

// HttpClientFactory : client nommé vers l'API d'authentification (génération du token)
builder.Services.AddHttpClient("AuthApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5211/api/");
});

// HttpClientFactory : client nommé vers la Gateway (listes des ordinateurs et des réparations)
builder.Services.AddHttpClient("Gateway", client =>
{
    client.BaseAddress = new Uri("http://localhost:5010/api/");
});

await builder.Build().RunAsync();
