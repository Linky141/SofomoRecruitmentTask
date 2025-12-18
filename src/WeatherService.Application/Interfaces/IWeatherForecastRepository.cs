using WeatherService.Domain.Entities;

namespace WeatherService.Application.Interfaces;

public interface IWeatherForecastRepository
{
    Task<WeatherForecast> AddAsync(WeatherForecast forecast);
    Task<WeatherForecast?> GetLatestByLocationIdAsync(int locationId);
}