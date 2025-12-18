namespace WeatherService.Domain.Entities;

public class Location
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<WeatherForecast> Forecasts { get; set; } = new();
}