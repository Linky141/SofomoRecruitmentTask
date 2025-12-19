namespace WeatherService.Application.Interfaces;

/// <summary>
/// Defines the contract for a service that provides weather data from an external API.
/// </summary>
public interface IWeatherApiService
{
    /// <summary>
    /// Retrieves the current temperature for a given latitude and longitude.
    /// </summary>
    /// <param name="latitude">Latitude of the location.</param>
    /// <param name="longitude">Longitude of the location.</param>
    /// <returns>The temperature in Celsius as a double.</returns>
    Task<double> GetTemperatureAsync(double latitude, double longitude);
}

/// <summary>
/// Represents a weather forecast DTO returned by the external weather API.
/// </summary>
/// <param name="ForecastDate">The date and time of the forecast.</param>
/// <param name="TemperatureC">The temperature in Celsius.</param>
public record WeatherApiForecastDto(
    DateTime ForecastDate,
    double TemperatureC
);