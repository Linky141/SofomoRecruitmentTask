using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;

public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, AddLocationResponseDto>
{
    private readonly ILocationRepository _locationRepo;
    private readonly IWeatherForecastRepository _forecastRepo;
    private readonly IWeatherApiService _weatherApi;

    public AddLocationCommandHandler(
        ILocationRepository locationRepo,
        IWeatherForecastRepository forecastRepo,
        IWeatherApiService weatherApi)
    {
        _locationRepo = locationRepo;
        _forecastRepo = forecastRepo;
        _weatherApi = weatherApi;
    }

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