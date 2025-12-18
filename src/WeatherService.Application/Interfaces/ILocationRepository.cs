using WeatherService.Domain.Entities;

namespace WeatherService.Application.Interfaces;

public interface ILocationRepository
{
    Task<Location> AddAsync(Location location);
    Task DeleteAsync(int id);
    Task<Location?> GetByIdAsync(int id);
    Task<IEnumerable<Location>> GetAllAsync();
}