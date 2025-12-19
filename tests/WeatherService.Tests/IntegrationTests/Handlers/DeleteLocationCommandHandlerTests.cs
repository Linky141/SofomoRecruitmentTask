using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

namespace WeatherService.Tests.IntegrationTests.Handlers
{
    public class DeleteLocationCommandHandlerTests
    {
        private WeatherDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new WeatherDbContext(options);
        }

        [Fact]
        public async Task Handle_Should_DeleteLocation_And_RelatedForecasts()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_DeleteLocation_And_RelatedForecasts));

            var location = new Location { Id = 1, Latitude = 10, Longitude = 20 };
            db.Locations.Add(location);

            db.WeatherForecasts.Add(new WeatherForecast
            {
                Id = 1,
                LocationId = 1,
                ForecastDate = DateTime.UtcNow,
                TemperatureC = 25
            });

            db.WeatherForecasts.Add(new WeatherForecast
            {
                Id = 2,
                LocationId = 1,
                ForecastDate = DateTime.UtcNow.AddHours(-1),
                TemperatureC = 22
            });

            await db.SaveChangesAsync();

            var locationRepo = new LocationRepository(db);
            var handler = new DeleteLocationCommandHandler(locationRepo);

            var command = new DeleteLocationCommand(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert 
            Assert.Equal(1, result.Id);
            Assert.Equal(10, result.Latitude);
            Assert.Equal(20, result.Longitude);

            // Assert 
            Assert.False(await db.Locations.AnyAsync(x => x.Id == 1));

            // Assert 
            Assert.False(await db.WeatherForecasts.AnyAsync(x => x.LocationId == 1));
        }

        [Fact]
        public async Task Handle_Should_Throw_WhenLocationDoesNotExist()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_Throw_WhenLocationDoesNotExist));

            var locationRepo = new LocationRepository(db);
            var handler = new DeleteLocationCommandHandler(locationRepo);

            var command = new DeleteLocationCommand(999);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );
        }
    }
}