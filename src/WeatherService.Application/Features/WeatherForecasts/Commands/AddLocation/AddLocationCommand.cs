using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;

public record AddLocationCommand(
    double Latitude,
    double Longitude,
    string Summary
) : IRequest<AddLocationResponseDto>;