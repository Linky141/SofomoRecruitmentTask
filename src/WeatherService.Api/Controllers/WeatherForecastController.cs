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
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        try
        {
            var deletedLocation = await _mediator.Send(new DeleteLocationCommand(id));
            return Ok(deletedLocation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetForecast(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetForecastByLocationIdQuery(id));
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred." });
        }
    }

    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        try
        {
            var result = await _mediator.Send(new GetAllLocationsQuery());
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An unexpected error occurred while retrieving locations." });
        }
    }
}