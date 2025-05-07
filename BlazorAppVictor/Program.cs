using BlazorAppVictor;
using BlazorAppVictor.Auth;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Añadir HttpClient para las solicitudes API
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Configurar servicios adicionales
ConfigureServices(builder.Services);

await builder.Build().RunAsync();

void ConfigureServices(IServiceCollection services)
{
    // Agregar SweetAlert2 para mensajes emergentes
    services.AddSweetAlert2();

    // Configurar el core de autorización
    services.AddAuthorizationCore(options =>
    {
        // Definir las políticas de autorización para los permisos
        options.AddPolicy("CrearCliente", policy => policy.RequireClaim("Permiso", "crearcliente"));
        options.AddPolicy("ModificarCliente", policy => policy.RequireClaim("Permiso", "modificarcliente"));
        options.AddPolicy("EliminarCliente", policy => policy.RequireClaim("Permiso", "eliminarcliente"));
        options.AddPolicy("CrearRecibo", policy => policy.RequireClaim("Permiso", "crearrecibo"));
        options.AddPolicy("ModificarRecibo", policy => policy.RequireClaim("Permiso", "modificarrecibo"));
        options.AddPolicy("EliminarRecibo", policy => policy.RequireClaim("Permiso", "eliminarrecibo"));
    });

    // Configuración del proveedor de autenticación JWT
    services.AddScoped<ProveedorAutenticacionJWT>();
    services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacionJWT>(proveedor =>
        proveedor.GetRequiredService<ProveedorAutenticacionJWT>());

    // Servicio de login
    services.AddScoped<ILoginService, ProveedorAutenticacionJWT>(proveedor =>
        proveedor.GetRequiredService<ProveedorAutenticacionJWT>());

    // ?? Registro del TokenService
    services.AddScoped<TokenService>();
}
