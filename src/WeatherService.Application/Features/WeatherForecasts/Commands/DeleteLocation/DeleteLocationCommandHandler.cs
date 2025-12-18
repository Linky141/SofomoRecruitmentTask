using MediatR;
using WeatherService.Application.Features.WeatherForecasts.Dtos;
using WeatherService.Application.Interfaces;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, LocationDto>
{
    private readonly ILocationRepository _locationRepo;

    public DeleteLocationCommandHandler(ILocationRepository locationRepo)
    {
        _locationRepo = locationRepo;
    }

    public async Task<LocationDto> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var location = await _locationRepo.GetByIdAsync(request.Id);
        if (location == null)
            throw new KeyNotFoundException($"Location with ID {request.Id} was not found.");

        await _locationRepo.DeleteAsync(request.Id);

        return new LocationDto(location.Id, location.Latitude, location.Longitude);
    }
}