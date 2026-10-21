using AC.Core.Shared.Models;
using Microsoft.Extensions.Options;

namespace AC.API.Extensions;

// Middleware que protege las escrituras en /api con el header X-Api-Key.
// Las lecturas (GET) son públicas para que el dashboard las consuma sin secretos.
public class ApiKeyAuthorizer(RequestDelegate next, IOptions<AppConfig> config)
{
    public const string HeaderName = "X-Api-Key";

    public async Task InvokeAsync(HttpContext context)
    {
        if (RequiresApiKey(context.Request))
        {
            var expectedKey = config.Value.ApiKey;
            context.Request.Headers.TryGetValue(HeaderName, out var receivedKey);

            // Falla cerrada: si la clave no está configurada, nadie escribe.
            if (string.IsNullOrEmpty(expectedKey) || receivedKey != expectedKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(
                    new { error = "Invalid or missing API key." });
                return;
            }
        }

        await next(context);
    }

    // Solo se pide la clave para escribir; GET, HEAD y el preflight de CORS pasan.
    private static bool RequiresApiKey(HttpRequest request) =>
        request.Path.StartsWithSegments("/api")
        && !HttpMethods.IsGet(request.Method)
        && !HttpMethods.IsHead(request.Method)
        && !HttpMethods.IsOptions(request.Method);
}
