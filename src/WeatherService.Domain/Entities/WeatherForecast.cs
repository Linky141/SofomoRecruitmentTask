namespace WeatherService.Domain.Entities;

public class WeatherForecast
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public DateTime ForecastDate { get; set; }
    public double TemperatureC { get; set; }
    public string Summary { get; set; }

    public Location Location { get; set; }
}