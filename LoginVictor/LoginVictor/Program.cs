using System.Text.Json.Serialization;
using LoginVictor.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Kestrel para especificar puertos
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7218, o => o.UseHttps()); // Configura el puerto HTTPS (7218)
    options.ListenAnyIP(5183); // Configura el puerto HTTP (5183)
});

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        // Permitir solicitudes desde el puerto de Blazor (5010 o cualquier otro que utilices)
        policy.WithOrigins("http://localhost:5010", "https://localhost:5010")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Área de servicios

// Habilitamos los controladores
builder.Services.AddControllers().AddJsonOptions(opciones =>
    opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configuración del ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseNpgsql("name=DefaultConnection"));

var app = builder.Build();

// Área de Middlewares
app.UseExceptionHandler("/Home/Error");
app.UseCors("AllowBlazor");
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
