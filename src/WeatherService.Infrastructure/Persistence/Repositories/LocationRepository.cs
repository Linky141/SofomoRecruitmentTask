using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for managing <see cref="Location"/> entities
/// using Entity Framework Core as the data access layer.
/// </summary>
public class LocationRepository : ILocationRepository
{
    private readonly WeatherDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationRepository"/> class.
    /// </summary>
    /// <param name="context">The EF Core database context used for data persistence.</param>
    public LocationRepository(WeatherDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new <see cref="Location"/> entity to the database.
    /// </summary>
    /// <param name="location">The location entity to be added.</param>
    /// <returns>
    /// The newly created <see cref="Location"/> including its generated ID.
    /// </returns>
    public async Task<Location> AddAsync(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return location;
    }

    /// <summary>
    /// Retrieves a location by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the location.</param>
    /// <returns>
    /// The <see cref="Location"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Location?> GetByIdAsync(int id)
    {
        return await _context.Locations.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all locations stored in the database.
    /// </summary>
    /// <returns>
    /// A collection of all <see cref="Location"/> entities.
    /// </returns>
    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _context.Locations.ToListAsync();
    }

    /// <summary>
    /// Deletes a location with the specified ID from the database.
    /// </summary>
    /// <param name="id">The ID of the location to delete.</param>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when a location with the specified ID does not exist.
    /// </exception>
    public async Task DeleteAsync(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null)
            throw new KeyNotFoundException($"Location with id {id} not found.");

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a location matching the given latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    /// <returns>
    /// The matching <see cref="Location"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Location?> GetByCoordinatesAsync(double latitude, double longitude)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);
    }
}