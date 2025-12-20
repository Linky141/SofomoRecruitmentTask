# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["WeatherService.Api/WeatherService.Api.csproj", "WeatherService.Api/"]
COPY ["WeatherService.Application/WeatherService.Application.csproj", "WeatherService.Application/"]
COPY ["WeatherService.Infrastructure/WeatherService.Infrastructure.csproj", "WeatherService.Infrastructure/"]
COPY ["WeatherService.Domain/WeatherService.Domain.csproj", "WeatherService.Domain/"]

RUN dotnet restore "WeatherService.Api/WeatherService.Api.csproj"

COPY . .

WORKDIR /src/WeatherService.Api
RUN dotnet publish "WeatherService.Api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WeatherService.Api.dll"]