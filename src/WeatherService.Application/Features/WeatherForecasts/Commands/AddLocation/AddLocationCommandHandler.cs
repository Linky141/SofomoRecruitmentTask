using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;

/// <summary>
/// Handles the <see cref="AddLocationCommand"/> by adding a new location and its initial weather forecast.
/// </summary>
public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, AddLocationResponseDto>
{
    private readonly ILocationRepository _locationRepo;
    private readonly IWeatherForecastRepository _forecastRepo;
    private readonly IWeatherApiService _weatherApi;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddLocationCommandHandler"/> class.
    /// </summary>
    /// <param name="locationRepo">Repository for accessing and storing location data.</param>
    /// <param name="forecastRepo">Repository for accessing and storing weather forecast data.</param>
    /// <param name="weatherApi">Service for retrieving weather information from an external API.</param>
    public AddLocationCommandHandler(
        ILocationRepository locationRepo,
        IWeatherForecastRepository forecastRepo,
        IWeatherApiService weatherApi)
    {
        _locationRepo = locationRepo;
        _forecastRepo = forecastRepo;
        _weatherApi = weatherApi;
    }

    /// <summary>
    /// Handles the command to add a location and its associated weather forecast.
    /// </summary>
    /// <param name="request">The command containing the latitude and longitude of the location.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="AddLocationResponseDto"/> containing the location ID, coordinates,
    /// forecast date, and temperature in Celsius.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when latitude is not between -90 and 90 or longitude is not between -180 and 180.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if there is an error saving the location or weather forecast to the database.
    /// </exception>
    public async Task<AddLocationResponseDto> Handle(AddLocationCommand request, CancellationToken cancellationToken)
    {
        if (request.Latitude < -90 || request.Latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(request.Latitude), "Latitude must be between -90 and 90.");

        if (request.Longitude < -180 || request.Longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(request.Longitude), "Longitude must be between -180 and 180.");

        var location = await _locationRepo.GetByCoordinatesAsync(request.Latitude, request.Longitude);

        if (location == null)
        {
            location = new Location
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            try
            {
                await _locationRepo.AddAsync(location);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to add location to database.", ex);
            }
        }

        double temperature;
        try
        {
            temperature = await _weatherApi.GetTemperatureAsync(request.Latitude, request.Longitude);
        }
        catch
        {
            temperature = 0;
        }

        var weather = new WeatherForecast
        {
            LocationId = location.Id,
            ForecastDate = DateTime.UtcNow,
            TemperatureC = temperature,
        };

        try
        {
            await _forecastRepo.AddAsync(weather);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to add weather forecast to database.", ex);
        }

        return new AddLocationResponseDto(
            location.Id,
            location.Latitude,
            location.Longitude,
            weather.ForecastDate,
            weather.TemperatureC
        );
    }
}