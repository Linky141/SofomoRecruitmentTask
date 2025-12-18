using System.Net.Http.Json;
using WeatherService.Application.Interfaces;

public class WeatherApiService : IWeatherApiService
{
    private readonly HttpClient _httpClient;

    public WeatherApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<double> GetTemperatureAsync(double latitude, double longitude)
    {
        var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);

        return response?.current_weather?.temperature ?? 0;
    }
}

public class OpenMeteoResponse
{
    public CurrentWeather? current_weather { get; set; }
}

public class CurrentWeather
{
    public double temperature { get; set; }
}