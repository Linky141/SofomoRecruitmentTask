using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

public class LocationRepository : ILocationRepository
{
    private readonly WeatherDbContext _context;

    public LocationRepository(WeatherDbContext context)
    {
        _context = context;
    }

    public async Task<Location> AddAsync(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();
        return location;
    }

    public async Task<Location?> GetByIdAsync(int id)
    {
        return await _context.Locations.FindAsync(id);
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _context.Locations.ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null)
            throw new KeyNotFoundException($"Location with id {id} not found.");

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
    }

    public async Task<Location?> GetByCoordinatesAsync(double latitude, double longitude)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(l => l.Latitude == latitude && l.Longitude == longitude);
    }
}