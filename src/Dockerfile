FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore ./WebApi/WebApi.csproj
RUN dotnet build ./WebApi/WebApi.csproj -c Release
RUN dotnet publish ./WebApi/WebApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8000 8001

ENTRYPOINT ["dotnet", "WebApi.dll"]
