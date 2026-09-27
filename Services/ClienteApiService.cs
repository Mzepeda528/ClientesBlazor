using System.Net.Http.Json;
using System.Text.Json;
using ClientesBlazor.Models;

namespace ClientesBlazor.Services
{
    public class ClienteApiService
    {
        private readonly HttpClient _http;

        // Case-insensitive para no depender de mayúsculas/minúsculas en el JSON de la API
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ClienteApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            var respuesta = await _http.GetAsync("Clientes");
            respuesta.EnsureSuccessStatusCode();

            var contenido = await respuesta.Content.ReadAsStreamAsync();
            var clientes = await JsonSerializer.DeserializeAsync<List<Cliente>>(contenido, JsonOptions);
            return clientes ?? new List<Cliente>();
        }

        public async Task<Cliente?> CrearClienteAsync(Cliente cliente)
        {
            var respuesta = await _http.PostAsJsonAsync("Clientes", cliente);
            respuesta.EnsureSuccessStatusCode();

            var contenido = await respuesta.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Cliente>(contenido, JsonOptions);
        }

        public async Task ActualizarClienteAsync(int id, Cliente cliente)
        {
            var respuesta = await _http.PutAsJsonAsync($"Clientes/{id}", cliente);
            respuesta.EnsureSuccessStatusCode();
        }

        public async Task EliminarClienteAsync(int id)
        {
            var respuesta = await _http.DeleteAsync($"Clientes/{id}");
            respuesta.EnsureSuccessStatusCode();
        }
    }
}
