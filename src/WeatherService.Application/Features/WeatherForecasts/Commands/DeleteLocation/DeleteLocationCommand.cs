using MediatR;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;

public record DeleteLocationCommand(int Id) : IRequest;