using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

namespace WeatherService.Infrastructure.Repositories;

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

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Locations.FindAsync(id);
        if (entity != null)
        {
            _context.Locations.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _context.Locations.ToListAsync();
    }

    public async Task<Location?> GetByIdAsync(int id)
    {
        return await _context.Locations.FindAsync(id);
    }
}