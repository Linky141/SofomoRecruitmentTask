namespace WeatherService.Application.Features.WeatherForecasts.Dtos;

public record AddLocationResponseDto(
    int LocationId,
    double Latitude,
    double Longitude,
    DateTime ForecastDate,
    double TemperatureC
);