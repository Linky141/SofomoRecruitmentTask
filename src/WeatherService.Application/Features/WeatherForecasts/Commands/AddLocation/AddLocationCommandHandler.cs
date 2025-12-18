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
    var location = new Location
    {
        Latitude = request.Latitude,
        Longitude = request.Longitude
    };
    await _locationRepo.AddAsync(location);

    var temperature = await _weatherApi.GetTemperatureAsync(request.Latitude, request.Longitude);

    var weather = new WeatherForecast
    {
        LocationId = location.Id,
        ForecastDate = DateTime.UtcNow, 
        TemperatureC = temperature,     
        Summary = request.Summary      
    };
    await _forecastRepo.AddAsync(weather);

    return new AddLocationResponseDto(
        location.Id,
        location.Latitude,
        location.Longitude,
        weather.ForecastDate,
        weather.TemperatureC,
        weather.Summary
    );
}
}