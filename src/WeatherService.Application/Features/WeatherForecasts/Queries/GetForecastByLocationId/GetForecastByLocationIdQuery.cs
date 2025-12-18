using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;

namespace WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;

public record GetForecastByLocationIdQuery(int Id)
    : IRequest<ForecastResponseDto>;