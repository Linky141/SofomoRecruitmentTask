namespace WeatherService.Application.Features.WeatherForecasts.Dtos;

public record LocationDto(
    int Id,
    double Latitude,
    double Longitude
);