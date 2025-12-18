using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;

public record DeleteLocationCommand(int Id) : IRequest<LocationDto>;