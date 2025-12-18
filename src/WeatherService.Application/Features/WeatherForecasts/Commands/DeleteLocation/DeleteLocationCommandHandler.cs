using MediatR;
using WeatherService.Application.Interfaces;

namespace WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand>
{
    private readonly ILocationRepository _locationRepo;

    public DeleteLocationCommandHandler(ILocationRepository locationRepo)
    {
        _locationRepo = locationRepo;
    }

    public async Task<Unit> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        await _locationRepo.DeleteAsync(request.Id);
        return Unit.Value;
    }
}