using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;
using WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;
using WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;
using WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;

namespace WeatherService.Api.Controllers;

/// <summary>
/// Controller responsible for handling weather-related operations such as adding, deleting locations 
/// and retrieving weather forecasts.
/// </summary>
[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling commands and queries.</param>
    public WeatherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Adds a new location.
    /// </summary>
    /// <param name="command">AddLocationCommand data.</param>
    /// <returns>
    /// Returns 200 OK with the added location result if successful, 
    /// 400 Bad Request if the input is invalid,
    /// 500 Internal Server Error for other exceptions.
    /// </returns>
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

    /// <summary>
    /// Deletes an existing location by ID.
    /// </summary>
    /// <param name="id">The ID of the location to delete.</param>
    /// <returns>
    /// Returns 200 OK with deleted location info if successful,
    /// 404 Not Found if the location does not exist,
    /// 500 Internal Server Error for other exceptions.
    /// </returns>
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

    /// <summary>
    /// Retrieves the weather forecast for a given location ID.
    /// </summary>
    /// <param name="id">The ID of the location.</param>
    /// <returns>
    /// Returns 200 OK with the forecast if found,
    /// 404 Not Found if the location does not exist,
    /// 500 Internal Server Error for other exceptions.
    /// </returns>
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

    /// <summary>
    /// Retrieves all available locations.
    /// </summary>
    /// <returns>
    /// Returns 200 OK with the list of locations if successful,
    /// 500 Internal Server Error for exceptions.
    /// </returns>
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