using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;

namespace WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;

public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDto>>
{
    private readonly ILocationRepository _locationRepo;

    public GetAllLocationsQueryHandler(ILocationRepository locationRepo)
    {
        _locationRepo = locationRepo;
    }

    public async Task<IEnumerable<LocationDto>> Handle(
        GetAllLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var locations = await _locationRepo.GetAllAsync();

        return locations.Select(x => new LocationDto(x.Id, x.Latitude, x.Longitude));
    }
}