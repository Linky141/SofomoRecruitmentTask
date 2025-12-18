namespace WeatherService.Application.Features.WeatherForecasts.Dtos;

public record ForecastResponseDto(
    int LocationId,
    double Latitude,
    double Longitude,
    DateTime ForecastDate,
    double TemperatureC,
    string Summary
);