namespace WeatherService.Application.Features.WeatherForecasts.Dtos;

/// <summary>
/// Data Transfer Object representing a location.
/// </summary>
/// <param name="Id">The unique identifier of the location.</param>
/// <param name="Latitude">The latitude coordinate of the location.</param>
/// <param name="Longitude">The longitude coordinate of the location.</param>
public record LocationDto(
    int Id,
    double Latitude,
    double Longitude
);