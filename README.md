# SocialNetwork Backend

ASP.NET Core 10 starter for a modular-monolith social network backend.

## Architecture

- `SocialNetwork.Api`: HTTP host, middleware, OpenAPI and application composition.
- `SocialNetwork.BuildingBlocks`: shared primitives only; avoid turning this into a dumping ground.
- `SocialNetwork.Modules.Identity`: identity-related module boundary and initial status endpoint.
- `SocialNetwork.Modules.Posts`: posts feature, domain entity and PostgreSQL persistence.
- `tests/SocialNetwork.UnitTests`: starter unit tests.

Each module owns its application features and persistence. The API host composes modules. Keep module-to-module communication behind explicit contracts/events; do not access another module's DbContext or tables directly.

## Requirements

- .NET SDK 10.0.100 or a compatible later .NET 10 feature-band SDK
- Docker Desktop (recommended for local PostgreSQL)

## Run

```powershell
docker compose up -d postgres
dotnet restore
dotnet build
dotnet run --project .\src\SocialNetwork.Api\SocialNetwork.Api.csproj
```

The API prints its local address. Open `/openapi/v1.json` for the generated OpenAPI document.

## Configuration

The local development connection string is in `src/SocialNetwork.Api/appsettings.Development.json`.
For another environment, set `ConnectionStrings__Postgres` through environment variables or a secret store. Never commit production credentials.

## Starter API

- `GET /health`
- `GET /api/auth/status`
- `GET /api/posts`
- `GET /api/posts/{id}`
- `POST /api/posts`
- `PUT /api/posts/{id}`
- `DELETE /api/posts/{id}`

Example:

```json
{
  "content": "Hello from my social network",
  "visibility": "Public"
}
```

## Database note

This starter calls `EnsureCreatedAsync` for a frictionless local first run. Before production, replace it with EF Core migrations (`dotnet ef migrations add InitialCreate ...`) and a controlled deployment migration process. `EnsureCreated` and migrations should not be mixed for the same database.

## Security note

The Identity module currently exposes only a status endpoint. It does not authenticate users yet. Do not expose write endpoints publicly in a production deployment until authentication, authorization, rate limiting, validation, moderation and upload security are implemented.
