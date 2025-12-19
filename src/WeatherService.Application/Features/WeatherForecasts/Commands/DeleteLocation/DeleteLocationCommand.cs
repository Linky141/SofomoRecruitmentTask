using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;

/// <summary>
/// Command to delete a location by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the location to delete.</param>
/// <returns>Returns the <see cref="LocationDto"/> of the deleted location.</returns>
public record DeleteLocationCommand(int Id) : IRequest<LocationDto>;