# Tabla comparativa de resultados – OWASP ZAP

**Aplicación analizada:** ClientesBlazor (Blazor Server, .NET 10) – `http://localhost:5200/clientes`
**Herramienta:** OWASP ZAP 2.17.0 – Escaneo Automatizado (spider tradicional + spider ajax con Firefox Headless, política Dev Standard)
**Informes:** `Informe_ZAP_Antes.html` (9 alertas: 5 Medio, 2 Bajo, 2 Informativo) y `Informe_ZAP_Despues.html`

## Resumen

| Nivel de riesgo | Antes (`Informe_ZAP_Antes.html`) | Después (`Informe_ZAP_Despues.html`) |
|---|---|---|
| Alto | 0 | 0 |
| Medio | 5 | 0 |
| Bajo | 2 | 0 |
| Informativo | 2 | 2 |
| **Total de tipos de alerta** | **9** | **2** |


## Detalle por alerta

| Alerta encontrada | Riesgo | Corrección realizada | Resultado final |
|---|---|---|---|
| CSP: Directiva Wildcard | Medio | Se definió una CSP completa en `Program.cs`. La única CSP que enviaba la app era `frame-ancestors 'self'` (la agrega Blazor por defecto); ahora cada directiva usa `'self'`, sin comodines. | Corregida |
| CSP: Failure to Define Directive with No Fallback | Medio | Se agregaron las directivas que no heredan de `default-src`: `form-action 'self'`, `base-uri 'self'`, `frame-ancestors 'self'`, además de `object-src 'none'`. | Corregida |
| CSP: script-src unsafe-inline | Medio | Se definió `script-src 'self'`. La app no tiene scripts en línea. | Corregida |
| CSP: style-src unsafe-inline | Medio | Se definió `style-src 'self'` y se movió el `style="background-color..."` de los modales a la clase `.modal-fondo` en `app.css`. | Corregida |
| Falta atributo de integridad de recursos secundarios | Medio | Bootstrap 5.3.3 (CSS y JS) dejó de cargarse desde `cdn.jsdelivr.net` y ahora se sirve localmente desde `wwwroot/lib/bootstrap`. Ya no hay recursos de terceros. | Corregida |
| Falta encabezado X-Content-Type-Options | Bajo | Se agregó `X-Content-Type-Options: nosniff` a todas las respuestas (middleware en `Program.cs`). | Corregida |
| Inclusión de archivos fuente JavaScript entre dominios | Bajo | Mismo cambio: `bootstrap.bundle.min.js` ahora es local, no de un dominio externo. | Corregida |
| Aplicación Web Moderna | Informativo | No requiere modificación. ZAP solo avisa que la página es una aplicación de tipo SPA/AJAX (Blazor), por eso usa el spider ajax. | Informativa |
| Respuesta de Gestión de Sesión Identificada | Informativo | No requiere modificación. Es la cookie antiforgery de ASP.NET Core (`HttpOnly`, `SameSite=Strict`); la app no tiene inicio de sesión. | Informativa |

## Cambios adicionales (no marcados por ZAP)
- `Referrer-Policy: strict-origin-when-cross-origin` y `Permissions-Policy` (cámara, micrófono y geolocalización desactivados).

## Limitaciones del análisis
- Blazor Server realiza el CRUD por una conexión en tiempo real (SignalR/WebSockets); el spider de ZAP no ve esas operaciones.
- Solo se analizó el proyecto `ClientesBlazor`. La API (`ClientesApi`) no se escaneó.

## Alertas que permanecen después del segundo análisis
Permanecen únicamente las 2 alertas informativas. No son vulnerabilidades y no requieren corrección:

- **Aplicación Web Moderna:** ZAP solo informa que la página es una aplicación tipo SPA/AJAX (Blazor Server) y que conviene usar el spider ajax, que ya se utilizó en ambos escaneos.
- **Respuesta de Gestión de Sesión Identificada:** ZAP detecta la cookie antiforgery de ASP.NET Core (`.AspNetCore.Antiforgery`). Ya se envía con `HttpOnly` y `SameSite=Strict`, y la aplicación no tiene inicio de sesión de usuarios, por lo que no hay sesiones que proteger.

No apareció ninguna alerta nueva en el segundo análisis. Ambos escaneos se hicieron con las mismas condiciones: URL `http://localhost:5200/clientes`, política Dev Standard, spider tradicional y spider ajax con Firefox Headless.
