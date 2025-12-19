using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;

/// <summary>
/// Query to retrieve all locations stored in the system.
/// </summary>
/// <remarks>
/// This query does not require any parameters and returns a collection of <see cref="LocationDto"/> objects.
/// </remarks>
public record GetAllLocationsQuery : IRequest<IEnumerable<LocationDto>>;