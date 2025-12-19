using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;

/// <summary>
/// Command representing a request to add a new location with specified coordinates.
/// </summary>
/// <param name="Latitude">The latitude of the location.</param>
/// <param name="Longitude">The longitude of the location.</param>
/// <returns>Returns an <see cref="AddLocationResponseDto"/> containing details of the added location.</returns>
public record AddLocationCommand(
    double Latitude,
    double Longitude
) : IRequest<AddLocationResponseDto>;