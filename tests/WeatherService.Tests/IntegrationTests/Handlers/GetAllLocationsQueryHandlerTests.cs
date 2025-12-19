using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

namespace WeatherService.Tests.IntegrationTests.Handlers
{
    public class GetAllLocationsQueryHandlerTests
    {
        private WeatherDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new WeatherDbContext(options);
        }

        [Fact]
        public async Task Handle_Should_ReturnAllLocations_WhenLocationsExist()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_ReturnAllLocations_WhenLocationsExist));

            db.Locations.Add(new Location { Id = 1, Latitude = 10, Longitude = 20 });
            db.Locations.Add(new Location { Id = 2, Latitude = 30, Longitude = 40 });
            await db.SaveChangesAsync();

            var locationRepo = new LocationRepository(db);
            var handler = new GetAllLocationsQueryHandler(locationRepo);

            // Act
            var result = (await handler.Handle(new GetAllLocationsQuery(), CancellationToken.None)).ToList();

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal(10, result[0].Latitude);
            Assert.Equal(20, result[0].Longitude);

            Assert.Equal(2, result[1].Id);
            Assert.Equal(30, result[1].Latitude);
            Assert.Equal(40, result[1].Longitude);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenNoLocationsExist()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_ReturnEmptyList_WhenNoLocationsExist));

            var locationRepo = new LocationRepository(db);
            var handler = new GetAllLocationsQueryHandler(locationRepo);

            // Act
            var result = await handler.Handle(new GetAllLocationsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_Should_NotThrow_WhenDatabaseIsEmpty()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_NotThrow_WhenDatabaseIsEmpty));

            var locationRepo = new LocationRepository(db);
            var handler = new GetAllLocationsQueryHandler(locationRepo);

            // Act + Assert
            var result = await handler.Handle(new GetAllLocationsQuery(), CancellationToken.None);
            Assert.Empty(result);
        }
    }
}