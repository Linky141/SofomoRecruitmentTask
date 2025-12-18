namespace WeatherService.Application.Interfaces;

public interface IWeatherApiService
{
    Task<double> GetTemperatureAsync(double latitude, double longitude);
}

public record WeatherApiForecastDto(
    DateTime ForecastDate,
    double TemperatureC,
    string Summary
);