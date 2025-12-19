namespace WeatherService.Application.Features.WeatherForecasts.Dtos;

/// <summary>
/// Data Transfer Object representing the response after adding a new location
/// along with its initial weather forecast.
/// </summary>
/// <param name="LocationId">The unique identifier of the newly added location.</param>
/// <param name="Latitude">The latitude coordinate of the location.</param>
/// <param name="Longitude">The longitude coordinate of the location.</param>
/// <param name="ForecastDate">The date and time when the forecast was retrieved or created.</param>
/// <param name="TemperatureC">The temperature in Celsius for the location at the forecast date.</param>
public record AddLocationResponseDto(
    int LocationId,
    double Latitude,
    double Longitude,
    DateTime ForecastDate,
    double TemperatureC
);