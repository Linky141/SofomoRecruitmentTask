using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;

public record GetAllLocationsQuery : IRequest<IEnumerable<LocationDto>>;