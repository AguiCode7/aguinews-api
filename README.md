# 🦅 AguiNews API

API en **.NET 10** con **Clean Architecture** en 3 capas y **PostgreSQL**, que guarda los resúmenes de noticias que genera el bot de **n8n**. Protegida con API key.

▶️ **Video paso a paso:** [API en .NET con Clean Architecture y PostgreSQL + n8n](https://youtu.be/CIM9Qja3Xow)
⬅️ Video anterior (el bot de noticias en n8n): [youtu.be/VuJmM_N0J8k](https://youtu.be/VuJmM_N0J8k) · Flujo: [n8n-daily-news-bot](https://github.com/AguiCode7/n8n-daily-news-bot)

## Cómo funciona

```
n8n (HTTP Request) ──POST /api/summaries + X-Api-Key──▶ AC.API ──▶ AC.Core ◀── AC.Infrastructure ──▶ PostgreSQL
```

| Capa | Proyecto | Qué hace |
| --- | --- | --- |
| API | `AC.API` | Controladores, seguridad con API key, manejo global de errores, Scalar |
| Core | `AC.Core` | Entidades, DTOs, puertos (interfaces), casos de uso y validación (FluentValidation). No depende de nadie |
| Infrastructure | `AC.Infrastructure` | Entity Framework Core + Npgsql, repositorios y migraciones |

Las dependencias apuntan hacia el centro: API e Infrastructure conocen a Core; Core no conoce a nadie.

### Endpoints

| Método | Ruta | Respuesta |
| --- | --- | --- |
| POST | `/api/summaries` | 201 con el resumen creado (400 si los datos no son válidos) |
| GET | `/api/summaries/{id}` | 200 o 404 |
| GET | `/api/summaries?count=5` | 200 con los últimos resúmenes, del más nuevo al más viejo |

Todas las rutas `/api` piden el header `X-Api-Key`. Sin clave (o si la clave no está configurada) responden 401.

## Requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [SDK de .NET 10](https://dotnet.microsoft.com/download)
- n8n (opcional, para conectar el flujo del bot)

## Uso paso a paso

1. **Base de datos**
   ```bash
   cp .env.example .env        # cambia la contraseña en .env (no se sube a git)
   docker compose up -d
   ```
2. **Secretos de desarrollo** (nunca en el código ni en appsettings)
   ```bash
   dotnet user-secrets set "ConnectionStrings:AguiNews" "Host=localhost;Port=5432;Database=aguinews;Username=agui;Password=TU_CLAVE" --project src/AC.API
   dotnet user-secrets set "AppConfig:ApiKey" "TU_API_KEY" --project src/AC.API
   ```
3. **Migración**
   ```bash
   dotnet tool restore         # o: dotnet tool install --global dotnet-ef
   dotnet ef database update --project src/AC.Infrastructure --startup-project src/AC.API
   ```
4. **Ejecutar**
   ```bash
   dotnet run --project src/AC.API
   ```
   Abre `http://localhost:5080/scalar` para probar los endpoints.

### Conectar n8n

Nodo **HTTP Request** después del nodo de OpenAI:

- Method: `POST` · URL: `http://host.docker.internal:5080/api/summaries`
- Authentication: Generic Credential Type → Header Auth (Name `X-Api-Key`, Value: tu clave)
- Send Body: JSON → Using Fields Below: `content`, `source`, `newsCount`

En Linux agrega al docker compose de n8n:

```yaml
extra_hosts:
  - "host.docker.internal:host-gateway"
```

## Configuración

| Dónde | Qué |
| --- | --- |
| `.env` | Base, usuario, contraseña y puerto de PostgreSQL (copia de `.env.example`) |
| user-secrets / variables de entorno | `ConnectionStrings:AguiNews` y `AppConfig:ApiKey` |
| `appsettings.json` | `AppConfig:MaxSummariesPerQuery` (máximo de resúmenes por consulta) |

`launchSettings.json` escucha en `0.0.0.0:5080` para que n8n en Docker pueda llegar a la API. Solo para desarrollo.

## Licencia

[MIT](LICENSE) · Hecho por [AguiCode](https://www.youtube.com/channel/UCOwIey0vGlYkZBgF29ASn3A): programación, automatización e IA en español.
