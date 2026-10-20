using AC.API.Extensions;
using AC.Core;
using AC.Core.Shared.Models;
using AC.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración tipada y capas
builder.Services.Configure<AppConfig>(
    builder.Configuration.GetSection(AppConfig.SectionName));
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Documentación interactiva en /scalar
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseMiddleware<ApiKeyAuthorizer>();
app.MapControllers();

app.Run();
