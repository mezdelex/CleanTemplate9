# .NET 8 Clean Architecture Template

.NET 8 Clean Architecture + DDD + CQRS + Specifications + AutoMapper + Domain Events + Testing + Identity + Redis

## Docker

`docker-compose up` @ project root

## Migrations

`dotnet ef migrations add <migration_name> --project .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\WebApi\WebApi.csproj`

## Database

`dotnet ef database update --project .\src\Infrastructure\Infrastructure.csproj --startup-project .\src\WebApi\WebApi.csproj` or just let `ApplyMigrations` extension migrate automatically on `run/watch`.

<sub>There's stuff that should've been ignored kept public on purpose, like docker-compose's `.env` variables, for discoverability purposes.</sub>
