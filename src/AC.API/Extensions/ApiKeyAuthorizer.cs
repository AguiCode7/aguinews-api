using AC.Core.Shared.Models;
using Microsoft.Extensions.Options;

namespace AC.API.Extensions;

// Middleware que protege las rutas /api con el header X-Api-Key.
public class ApiKeyAuthorizer(RequestDelegate next, IOptions<AppConfig> config)
{
    public const string HeaderName = "X-Api-Key";

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            var expectedKey = config.Value.ApiKey;
            context.Request.Headers.TryGetValue(HeaderName, out var receivedKey);

            // Falla cerrada: si la clave no está configurada, nadie entra.
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
}
