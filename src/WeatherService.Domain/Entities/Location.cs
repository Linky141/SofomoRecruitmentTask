namespace WeatherService.Domain.Entities;

/// <summary>
/// Represents a geographic location for which weather forecasts can be stored.
/// </summary>
public class Location
{
    /// <summary>
    /// Gets or sets the unique identifier of the location.
    /// </summary>
    /// <remarks>Assigned by the persistence layer.</remarks>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate of the location.
    /// </summary>
    /// <value>A double representing geographic latitude.</value>
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate of the location.
    /// </summary>
    /// <value>A double representing geographic longitude.</value>
    public double Longitude { get; set; }

    /// <summary>
    /// Collection of weather forecasts associated with this location.
    /// </summary>
    /// <remarks>
    /// This list is typically managed by the ORM (e.g., EF Core).
    /// Contains zero or more <see cref="WeatherForecast"/> entries.
    /// </remarks>
    public List<WeatherForecast> Forecasts { get; set; } = new();
}