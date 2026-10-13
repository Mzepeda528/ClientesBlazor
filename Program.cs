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

// ---------------------------------------------------------------------------
// Cabeceras de seguridad (correcciones del análisis con OWASP ZAP)
// ---------------------------------------------------------------------------
// Política de Seguridad de Contenido (CSP):
//  - Todo el contenido (scripts, estilos, fuentes, imágenes) solo puede venir del propio sitio.
//  - Bootstrap ahora se sirve localmente (wwwroot/lib/bootstrap), ya no desde un CDN.
//  - 'data:' en img-src porque Bootstrap trae íconos SVG incrustados en su CSS.
//  - connect-src 'self' permite la conexión en tiempo real (WebSocket/SignalR) de Blazor Server.
//  - No se usa 'unsafe-inline' ni comodines (*).
var csp = string.Join("; ", new[]
{
    "default-src 'self'",
    "script-src 'self'",
    "style-src 'self'",
    "img-src 'self' data:",
    "font-src 'self'",
    "connect-src 'self'",
    "object-src 'none'",
    "base-uri 'self'",
    "form-action 'self'",
    "frame-ancestors 'self'"
});

app.Use(async (context, next) =>
{
    // OnStarting se ejecuta justo antes de enviar las cabeceras, así que nuestra CSP
    // reemplaza la que Blazor agrega por defecto (solo "frame-ancestors 'self'").
    context.Response.OnStarting(() =>
    {
        var headers = context.Response.Headers;
        headers["Content-Security-Policy"] = csp;
        headers["X-Content-Type-Options"] = "nosniff";
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
        return Task.CompletedTask;
    });

    await next();
});

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
