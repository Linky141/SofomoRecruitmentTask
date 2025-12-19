using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;

namespace WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;

/// <summary>
/// Handles the <see cref="GetForecastByLocationIdQuery"/> by retrieving the latest weather forecast
/// for a specific location.
/// </summary>
public class GetForecastByLocationIdQueryHandler : IRequestHandler<GetForecastByLocationIdQuery, ForecastResponseDto>
{
    private readonly IWeatherForecastRepository _forecastRepo;
    private readonly ILocationRepository _locationRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetForecastByLocationIdQueryHandler"/> class.
    /// </summary>
    /// <param name="forecastRepo">Repository for accessing weather forecast data.</param>
    /// <param name="locationRepo">Repository for accessing location data.</param>
    public GetForecastByLocationIdQueryHandler(
        IWeatherForecastRepository forecastRepo,
        ILocationRepository locationRepo)
    {
        _forecastRepo = forecastRepo;
        _locationRepo = locationRepo;
    }

    /// <summary>
    /// Handles the query to get the latest weather forecast for a given location ID.
    /// </summary>
    /// <param name="request">The query containing the location ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="ForecastResponseDto"/> containing the location ID, coordinates, forecast date,
    /// and temperature in Celsius.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the location or its forecast does not exist in the database.
    /// </exception>
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