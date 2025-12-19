using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;

/// <summary>
/// Handles the <see cref="DeleteLocationCommand"/> to remove a location from the system.
/// </summary>
public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, LocationDto>
{
    private readonly ILocationRepository _locationRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLocationCommandHandler"/> class.
    /// </summary>
    /// <param name="locationRepo">Repository for accessing location data.</param>
    public DeleteLocationCommandHandler(ILocationRepository locationRepo)
    {
        _locationRepo = locationRepo;
    }

    /// <summary>
    /// Handles the deletion of a location.
    /// </summary>
    /// <param name="request">The command containing the ID of the location to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Returns a <see cref="LocationDto"/> representing the deleted location.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the location with the specified ID does not exist.</exception>
    public async Task<LocationDto> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var location = await _locationRepo.GetByIdAsync(request.Id);
        if (location == null)
            throw new KeyNotFoundException($"Location with ID {request.Id} was not found.");

        await _locationRepo.DeleteAsync(request.Id);

        return new LocationDto(location.Id, location.Latitude, location.Longitude);
    }
}