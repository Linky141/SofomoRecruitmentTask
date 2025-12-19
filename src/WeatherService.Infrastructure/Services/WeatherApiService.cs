using System.Net.Http.Json;
using WeatherService.Application.Interfaces;

/// <summary>
/// Service responsible for communicating with the external Open-Meteo API
/// to retrieve real-time weather temperature data.
/// </summary>
public class WeatherApiService : IWeatherApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of <see cref="WeatherApiService"/>.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send requests to the weather API.</param>
    public WeatherApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves the current temperature for the given geographic coordinates using the Open-Meteo API.
    /// </summary>
    /// <param name="latitude">Latitude value of the desired location.</param>
    /// <param name="longitude">Longitude value of the desired location.</param>
    /// <returns>
    /// A <see cref="double"/> representing the current temperature in Celsius.  
    /// Returns <c>0</c> if the API response is null or incomplete.
    /// </returns>
    public async Task<double> GetTemperatureAsync(double latitude, double longitude)
    {
        var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);

        return response?.current_weather?.temperature ?? 0;
    }
}

/// <summary>
/// Represents the response structure returned by the Open-Meteo API.
/// </summary>
public class OpenMeteoResponse
{
    /// <summary>
    /// Contains the current weather data returned by the API.
    /// </summary>
    public CurrentWeather? current_weather { get; set; }
}

/// <summary>
/// Represents the "current_weather" JSON object in the Open-Meteo API response.
/// </summary>
public class CurrentWeather
{
    /// <summary>
    /// Gets or sets the temperature in Celsius returned by the API.
    /// </summary>
    public double temperature { get; set; }
}