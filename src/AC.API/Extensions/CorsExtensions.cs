using AC.Core.Shared.Models;

namespace AC.API.Extensions;

// Registro de CORS: solo los orígenes configurados pueden leer la API.
public static class CorsExtensions
{
    public const string DashboardPolicy = "Dashboard";

    public static IServiceCollection AddDashboardCors(
        this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration
            .GetSection(AppConfig.SectionName)
            .Get<AppConfig>()?.AllowedOrigins ?? [];

        services.AddCors(options => options.AddPolicy(DashboardPolicy, policy =>
            policy.WithOrigins(origins)
                  .WithMethods(HttpMethods.Get)
                  .AllowAnyHeader()));

        return services;
    }
}
