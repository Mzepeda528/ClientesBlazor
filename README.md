# ClientesBlazor

Aplicación Blazor Server que consume la API `ClientesApi` (.NET) para administrar clientes.
No accede a la base de datos directamente: todo pasa por la API vía HTTP.

## Requisitos
- .NET SDK 10 instalado (el mismo que usaste para la API)
- Visual Studio Code con la extensión "C# Dev Kit"
- La API `ClientesApi` y su base de datos MySQL ya funcionando

## Cómo correr los DOS proyectos juntos en VS Code

Necesitas tener **ambos proyectos corriendo al mismo tiempo**, cada uno en su propia terminal,
porque Blazor le hace peticiones HTTP a la API mientras la usas.

1. Abre una carpeta padre en VS Code que contenga ambas carpetas de proyecto:
   ```
   MiTarea/
     ClientesApi/       <- tu API existente
     ClientesBlazor/     <- este proyecto
   ```

2. Abre una terminal en VS Code (Terminal > New Terminal) y entra a la API:
   ```
   cd ClientesApi
   dotnet run
   ```
   Debe quedar escuchando en `http://localhost:5177` (revisa el mensaje en consola,
   confirma que ese es el puerto; si cambia, ajústalo en el paso 4).

3. Abre una **segunda** terminal (ícono de "+" en el panel de terminal) sin cerrar la primera,
   y entra al proyecto Blazor:
   ```
   cd ClientesBlazor
   dotnet run
   ```
   Debe quedar escuchando en `http://localhost:5200`.

4. Si tu API corre en un puerto distinto a 5177, edita
   `ClientesBlazor/appsettings.json` y cambia:
   ```json
   "ApiSettings": { "BaseUrl": "http://localhost:TU_PUERTO/api/" }
   ```

5. Abre el navegador en `http://localhost:5200`, ve a la sección **Clientes** y prueba
   agregar, editar, eliminar y visualizar clientes.

## Notas
- Si al agregar/editar te sale "No se pudo conectar con la API", verifica que la terminal
  de `ClientesApi` siga corriendo y que el puerto coincida con el de `appsettings.json`.
- El proyecto usa Bootstrap 5 vía CDN, por lo que necesitas conexión a internet
  la primera vez que cargues la página (o descarga bootstrap localmente si tu
  entrega debe ser 100% offline).
