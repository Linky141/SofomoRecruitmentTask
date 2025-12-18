using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;
using WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;
using WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;
using WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;

namespace WeatherService.Api.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddLocation([FromBody] AddLocationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        await _mediator.Send(new DeleteLocationCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetForecast(int id)
    {
        var result = await _mediator.Send(new GetForecastByLocationIdQuery(id));
        return Ok(result);
    }

    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        var result = await _mediator.Send(new GetAllLocationsQuery());
        return Ok(result);
    }
}