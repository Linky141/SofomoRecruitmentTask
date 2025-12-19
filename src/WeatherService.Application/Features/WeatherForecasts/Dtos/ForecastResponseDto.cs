namespace WeatherService.Application.Features.WeatherForecasts.Dtos;

/// <summary>
/// Data Transfer Object representing a weather forecast for a specific location.
/// </summary>
/// <param name="LocationId">The unique identifier of the location.</param>
/// <param name="Latitude">The latitude coordinate of the location.</param>
/// <param name="Longitude">The longitude coordinate of the location.</param>
/// <param name="ForecastDate">The date and time of the forecast.</param>
/// <param name="TemperatureC">The temperature in Celsius for the forecast.</param>
public record ForecastResponseDto(
    int LocationId,
    double Latitude,
    double Longitude,
    DateTime ForecastDate,
    double TemperatureC
);