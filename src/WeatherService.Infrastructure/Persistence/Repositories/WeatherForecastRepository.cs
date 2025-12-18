using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

namespace WeatherService.Infrastructure.Repositories;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly WeatherDbContext _context;

    public WeatherForecastRepository(WeatherDbContext context)
    {
        _context = context;
    }

    public async Task<WeatherForecast> AddAsync(WeatherForecast forecast)
    {
        _context.WeatherForecasts.Add(forecast);
        await _context.SaveChangesAsync();
        return forecast;
    }

    public async Task<WeatherForecast?> GetLatestByLocationIdAsync(int locationId)
    {
        return await _context.WeatherForecasts
            .Where(f => f.LocationId == locationId)
            .OrderByDescending(f => f.ForecastDate)
            .FirstOrDefaultAsync();
    }
}