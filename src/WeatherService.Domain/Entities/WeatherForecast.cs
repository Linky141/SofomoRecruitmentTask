namespace WeatherService.Domain.Entities;

/// <summary>
/// Represents a weather forecast entry for a specific location and date.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Gets or sets the unique identifier of the weather forecast.
    /// </summary>
    /// <remarks>Assigned by the persistence layer.</remarks>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the associated location's identifier.
    /// </summary>
    /// <remarks>
    /// Used as a foreign key mapping to <see cref="Location"/>.
    /// </remarks>
    public int LocationId { get; set; }

    /// <summary>
    /// Gets or sets the date and time for which the weather forecast applies.
    /// </summary>
    public DateTime ForecastDate { get; set; }

    /// <summary>
    /// Gets or sets the temperature (in Celsius) predicted for the given date.
    /// </summary>
    public double TemperatureC { get; set; }

    /// <summary>
    /// Gets or sets the navigation property for the related location entity.
    /// </summary>
    /// <remarks>
    /// Typically populated by the ORM (e.g., EF Core) when loading related data.
    /// </remarks>
    public Location Location { get; set; }
}