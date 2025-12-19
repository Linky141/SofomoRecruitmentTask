using WeatherService.Domain.Entities;

namespace WeatherService.Application.Interfaces;

/// <summary>
/// Defines the contract for a repository managing location entities.
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// Adds a new location to the repository.
    /// </summary>
    /// <param name="location">The location entity to add.</param>
    /// <returns>The added <see cref="Location"/> entity.</returns>
    Task<Location> AddAsync(Location location);

    /// <summary>
    /// Deletes a location from the repository by its ID.
    /// </summary>
    /// <param name="id">The ID of the location to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a location by its ID.
    /// </summary>
    /// <param name="id">The ID of the location to retrieve.</param>
    /// <returns>The <see cref="Location"/> entity if found; otherwise, null.</returns>
    Task<Location?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all locations from the repository.
    /// </summary>
    /// <returns>An enumerable of all <see cref="Location"/> entities.</returns>
    Task<IEnumerable<Location>> GetAllAsync();

    /// <summary>
    /// Retrieves a location by its coordinates.
    /// </summary>
    /// <param name="latitude">Latitude of the location.</param>
    /// <param name="longitude">Longitude of the location.</param>
    /// <returns>The <see cref="Location"/> entity if found; otherwise, null.</returns>
    Task<Location?> GetByCoordinatesAsync(double latitude, double longitude);
}