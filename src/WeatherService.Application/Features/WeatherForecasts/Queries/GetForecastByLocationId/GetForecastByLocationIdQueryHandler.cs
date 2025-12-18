using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;

namespace WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;

public class GetForecastByLocationIdQueryHandler : IRequestHandler<GetForecastByLocationIdQuery, ForecastResponseDto>
{
    private readonly IWeatherForecastRepository _forecastRepo;
    private readonly ILocationRepository _locationRepo;

    public GetForecastByLocationIdQueryHandler(
        IWeatherForecastRepository forecastRepo,
        ILocationRepository locationRepo)
    {
        _forecastRepo = forecastRepo;
        _locationRepo = locationRepo;
    }

    public async Task<ForecastResponseDto> Handle(GetForecastByLocationIdQuery request, CancellationToken cancellationToken)
    {
        var location = await _locationRepo.GetByIdAsync(request.Id);
        if (location == null)
            throw new KeyNotFoundException($"Location with ID {request.Id} not found.");

        var forecast = await _forecastRepo.GetLatestByLocationIdAsync(request.Id);
        if (forecast == null)
            throw new KeyNotFoundException($"No forecast found for location ID {request.Id}.");

        return new ForecastResponseDto(
            location.Id,
            location.Latitude,
            location.Longitude,
            forecast.ForecastDate,
            forecast.TemperatureC
        );
    }
}