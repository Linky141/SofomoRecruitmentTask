using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;

/// <summary>
/// Query for retrieving the latest weather forecast for a specific location by its ID.
/// </summary>
/// <param name="Id">The unique identifier of the location.</param>
/// <returns>
/// A <see cref="ForecastResponseDto"/> containing the location ID, coordinates, forecast date,
/// and temperature in Celsius.
/// </returns>
public record GetForecastByLocationIdQuery(int Id)
    : IRequest<ForecastResponseDto>;