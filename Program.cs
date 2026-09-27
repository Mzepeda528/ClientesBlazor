using ClientesBlazor.Components;
using ClientesBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Componentes Razor con render interactivo en el servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Cliente HTTP para consumir la API de Clientes (ClientesApi)
// La URL base se lee de appsettings.json -> ApiSettings:BaseUrl
builder.Services.AddHttpClient<ClienteApiService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5177/api/";
    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
