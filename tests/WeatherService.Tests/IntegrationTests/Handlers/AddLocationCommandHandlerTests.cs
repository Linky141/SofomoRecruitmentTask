using Microsoft.EntityFrameworkCore;
using Moq;
using WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;
using WeatherService.Application.Interfaces;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Infrastructure.Repositories;

namespace WeatherService.Tests.IntegrationTests.Handlers
{
    public class AddLocationCommandHandlerTests
    {
        private WeatherDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new WeatherDbContext(options);
        }

        [Fact]
        public async Task Handle_Should_AddLocation_And_Forecast_WhenValidCoordinates()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_AddLocation_And_Forecast_WhenValidCoordinates));

            var locationRepo = new LocationRepository(db);
            var forecastRepo = new WeatherForecastRepository(db);

            var apiMock = new Mock<IWeatherApiService>();
            apiMock.Setup(x => x.GetTemperatureAsync(10, 20))
                   .ReturnsAsync(25);

            var handler = new AddLocationCommandHandler(
                locationRepo,
                forecastRepo,
                apiMock.Object
            );

            var command = new AddLocationCommand(10, 20);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert 
            Assert.True(result.LocationId > 0);
            Assert.Equal(25, result.TemperatureC);

            // Assert 
            var savedLocation = await db.Locations.FirstOrDefaultAsync();
            Assert.NotNull(savedLocation);
            Assert.Equal(10, savedLocation.Latitude);
            Assert.Equal(20, savedLocation.Longitude);

            // Assert 
            var savedForecast = await db.WeatherForecasts.FirstOrDefaultAsync();
            Assert.NotNull(savedForecast);
            Assert.Equal(25, savedForecast.TemperatureC);
            Assert.Equal(savedLocation.Id, savedForecast.LocationId);
        }

        [Fact]
        public async Task Handle_Should_AddForecast_WhenLocationAlreadyExists()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_AddForecast_WhenLocationAlreadyExists));

            var existingLocation = new Domain.Entities.Location
            {
                Latitude = 10,
                Longitude = 20
            };
            db.Locations.Add(existingLocation);
            await db.SaveChangesAsync();

            var locationRepo = new LocationRepository(db);
            var forecastRepo = new WeatherForecastRepository(db);
            var apiMock = new Mock<IWeatherApiService>();
            apiMock.Setup(x => x.GetTemperatureAsync(10, 20)).ReturnsAsync(25);

            var handler = new AddLocationCommandHandler(
                locationRepo,
                forecastRepo,
                apiMock.Object
            );

            var command = new AddLocationCommand(10, 20);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(existingLocation.Id, result.LocationId);
            Assert.Equal(25, result.TemperatureC);
            Assert.Equal(1, db.Locations.Count());
            Assert.Single(db.WeatherForecasts);
            Assert.Equal(existingLocation.Id, db.WeatherForecasts.First().LocationId);
        }

        [Fact]
        public async Task Handle_Should_AddForecastWithZeroTemperature_WhenApiFails()
        {
            // Arrange
            var db = CreateDbContext(nameof(Handle_Should_AddForecastWithZeroTemperature_WhenApiFails));

            var locationRepo = new LocationRepository(db);
            var forecastRepo = new WeatherForecastRepository(db);

            var apiMock = new Mock<IWeatherApiService>();
            apiMock.Setup(x => x.GetTemperatureAsync(10, 20))
                   .ThrowsAsync(new Exception("API error")); 

            var handler = new AddLocationCommandHandler(
                locationRepo,
                forecastRepo,
                apiMock.Object
            );

            var command = new AddLocationCommand(10, 20);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(10, result.Latitude);
            Assert.Equal(20, result.Longitude);
            Assert.Equal(0, result.TemperatureC);               
            Assert.Single(db.Locations);                        
            Assert.Single(db.WeatherForecasts);                
            Assert.Equal(db.Locations.First().Id, result.LocationId);
        }
    }
}