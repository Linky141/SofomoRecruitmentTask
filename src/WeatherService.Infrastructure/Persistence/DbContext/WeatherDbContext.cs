using Microsoft.EntityFrameworkCore;
using WeatherService.Domain.Entities;

namespace WeatherService.Infrastructure.Persistence;

/// <summary>
/// Represents the Entity Framework Core database context for the Weather Service.
/// Provides access to <see cref="Location"/> and <see cref="WeatherForecast"/> entities
/// and configures their mappings and relationships.
/// </summary>
public class WeatherDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context configuration options.</param>
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the database table for <see cref="WeatherForecast"/> entities.
    /// </summary>
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    /// <summary>
    /// Gets or sets the database table for <see cref="Location"/> entities.
    /// </summary>
    public DbSet<Location> Locations => Set<Location>();

    /// <summary>
    /// Configures the EF Core model, entity mappings, constraints,
    /// and relationships between domain entities.
    /// </summary>
    /// <param name="modelBuilder">The builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
            .HasOne(e => e.Location)
            .WithMany(l => l.Forecasts)
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.ForecastDate).IsRequired();
            entity.Property(e => e.TemperatureC).IsRequired();
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
        });
    }
}