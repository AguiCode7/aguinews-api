namespace AC.Core.Shared.Models;

// Configuración tipada de la aplicación (Options pattern).
// Los valores se leen de appsettings, user-secrets o variables de entorno.
public class AppConfig
{
    public const string SectionName = "AppConfig";

    public string ApiKey { get; set; } = string.Empty;
    public int MaxSummariesPerQuery { get; set; } = 50;

    // Orígenes que pueden leer la API desde el navegador (CORS).
    public string[] AllowedOrigins { get; set; } = [];
}
