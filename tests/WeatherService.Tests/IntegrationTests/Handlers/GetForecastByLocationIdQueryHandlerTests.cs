
using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Infrastructure.Repositories;

namespace WeatherService.Tests.IntegrationTests.Handlers
{
    public class GetForecastByLocationIdQueryHandlerTests
    {
        private WeatherDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new WeatherDbContext(options);
        }

        [Fact]
        public async Task Handle_Should_ReturnLatestForecast_WhenLocationAndForecastExist()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_ReturnLatestForecast_WhenLocationAndForecastExist));

            var location = new Location { Id = 1, Latitude = 50, Longitude = 20 };
            db.Locations.Add(location);

            db.WeatherForecasts.Add(new WeatherForecast
            {
                Id = 1,
                LocationId = 1,
                ForecastDate = DateTime.UtcNow.AddHours(-3),
                TemperatureC = 10
            });

            db.WeatherForecasts.Add(new WeatherForecast
            {
                Id = 2,
                LocationId = 1,
                ForecastDate = DateTime.UtcNow, 
                TemperatureC = 15
            });

            await db.SaveChangesAsync();

            var locationRepo = new LocationRepository(db);
            var forecastRepo = new WeatherForecastRepository(db);

            var handler = new GetForecastByLocationIdQueryHandler(forecastRepo, locationRepo);

            var query = new GetForecastByLocationIdQuery(1);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.LocationId);
            Assert.Equal(50, result.Latitude);
            Assert.Equal(20, result.Longitude);
            Assert.Equal(15, result.TemperatureC); 
        }

        [Fact]
        public async Task Handle_Should_Throw_WhenLocationDoesNotExist()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_Throw_WhenLocationDoesNotExist));

            var locationRepo = new LocationRepository(db);
            var forecastRepo = new WeatherForecastRepository(db);

            var handler = new GetForecastByLocationIdQueryHandler(forecastRepo, locationRepo);

            var query = new GetForecastByLocationIdQuery(999);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await handler.Handle(query, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_Should_Throw_WhenForecastDoesNotExistForLocation()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_Throw_WhenForecastDoesNotExistForLocation));

            var location = new Location { Id = 1, Latitude = 10, Longitude = 10 };
            db.Locations.Add(location);

            await db.SaveChangesAsync();

            var locationRepo = new LocationRepository(db);
            var forecastRepo = new WeatherForecastRepository(db);

            var handler = new GetForecastByLocationIdQueryHandler(forecastRepo, locationRepo);

            var query = new GetForecastByLocationIdQuery(1);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await handler.Handle(query, CancellationToken.None)
            );
        }
    }
}