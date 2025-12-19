using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;

namespace WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;

/// <summary>
/// Handles the <see cref="GetAllLocationsQuery"/> to retrieve all stored locations.
/// </summary>
public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDto>>
{
    private readonly ILocationRepository _locationRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllLocationsQueryHandler"/> class.
    /// </summary>
    /// <param name="locationRepo">Repository for accessing location data.</param>
    public GetAllLocationsQueryHandler(ILocationRepository locationRepo)
    {
        _locationRepo = locationRepo;
    }

    /// <summary>
    /// Handles the query to get all locations.
    /// </summary>
    /// <param name="request">The <see cref="GetAllLocationsQuery"/> request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// An <see cref="IEnumerable{LocationDto}"/> containing all locations with their ID, latitude, and longitude.
    /// </returns>
    public async Task<IEnumerable<LocationDto>> Handle(
        GetAllLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var locations = await _locationRepo.GetAllAsync();

        return locations.Select(x => new LocationDto(x.Id, x.Latitude, x.Longitude));
    }
}