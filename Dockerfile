# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/WeatherService.Api/WeatherService.Api.csproj", "WeatherService.Api/"]
COPY ["src/WeatherService.Application/WeatherService.Application.csproj", "WeatherService.Application/"]
COPY ["src/WeatherService.Infrastructure/WeatherService.Infrastructure.csproj", "WeatherService.Infrastructure/"]
COPY ["src/WeatherService.Domain/WeatherService.Domain.csproj", "WeatherService.Domain/"]

RUN dotnet restore "WeatherService.Api/WeatherService.Api.csproj"

COPY ./src .

WORKDIR /src/WeatherService.Api
RUN dotnet publish "WeatherService.Api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WeatherService.Api.dll"]