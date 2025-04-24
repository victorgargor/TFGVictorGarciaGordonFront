using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LoginVictor.Client;
using CurrieTechnologies.Razor.SweetAlert2;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddSweetAlert2();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7218/") 
});

await builder.Build().RunAsync();
