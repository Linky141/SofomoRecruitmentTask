namespace WeatherService.Domain.ValueObjects;

/// <summary>
/// Represents a geographical coordinate pair consisting of latitude and longitude.
/// </summary>
/// <param name="Latitude">
/// The latitude component of the coordinates.  
/// Expected range: -90 to 90 degrees.
/// </param>
/// <param name="Longitude">
/// The longitude component of the coordinates.  
/// Expected range: -180 to 180 degrees.
/// </param>
public record Coordinates(double Latitude, double Longitude);