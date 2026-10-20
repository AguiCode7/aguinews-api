using AC.Core.Modules.Summaries.Ports;
using AC.Core.Modules.Summaries.UseCases;
using AC.Core.Modules.Summaries.Validators;
using System.Globalization;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AC.Core;

// Registro de dependencias de la capa Core.
public static class Startup
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // Mensajes de validación siempre en inglés.
        ValidatorOptions.Global.LanguageManager.Culture =
            CultureInfo.InvariantCulture;

        services.AddValidatorsFromAssemblyContaining<CreateSummaryValidator>();
        services.AddScoped<ISummaryService, SummaryService>();
        return services;
    }
}
