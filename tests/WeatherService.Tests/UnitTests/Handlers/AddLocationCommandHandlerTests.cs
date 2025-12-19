using Moq;
using WeatherService.Application.Features.WeatherForecasts.Commands.AddLocation;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;

namespace WeatherService.Tests.Handlers
{
    public class AddLocationCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_AddLocationAndForecast_WhenCoordinatesAreValid()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();
            var forecastRepoMock = new Mock<IWeatherForecastRepository>();
            var weatherApiMock = new Mock<IWeatherApiService>();

            var command = new AddLocationCommand(10, 20);

            locationRepoMock.Setup(r => r.GetByCoordinatesAsync(command.Latitude, command.Longitude))
                            .ReturnsAsync((Location)null);

            locationRepoMock.Setup(r => r.AddAsync(It.IsAny<Location>()))
                            .ReturnsAsync((Location loc) => { loc.Id = 1; return loc; });

            weatherApiMock.Setup(w => w.GetTemperatureAsync(command.Latitude, command.Longitude))
                          .ReturnsAsync(25);

            forecastRepoMock.Setup(f => f.AddAsync(It.IsAny<WeatherForecast>()))
                            .ReturnsAsync((WeatherForecast wf) => wf);

            var handler = new AddLocationCommandHandler(
                locationRepoMock.Object,
                forecastRepoMock.Object,
                weatherApiMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.LocationId);  
            Assert.Equal(25, result.TemperatureC); 
        }

        [Theory]
        [InlineData(-91, 0)]
        [InlineData(91, 0)]
        [InlineData(0, -181)]
        [InlineData(0, 181)]
        public async Task Handle_Should_ThrowArgumentOutOfRangeException_WhenCoordinatesAreInvalid(double latitude, double longitude)
        {
            // Arrange
            var command = new AddLocationCommand(latitude, longitude);
            var handler = new AddLocationCommandHandler(
                Mock.Of<ILocationRepository>(),
                Mock.Of<IWeatherForecastRepository>(),
                Mock.Of<IWeatherApiService>()
            );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_UseExistingLocation_WhenCoordinatesAlreadyExist()
        {
            // Arrange
            var existingLocation = new Location { Id = 5, Latitude = 10, Longitude = 20 };
            var locationRepoMock = new Mock<ILocationRepository>();
            var forecastRepoMock = new Mock<IWeatherForecastRepository>();
            var weatherApiMock = new Mock<IWeatherApiService>();

            var command = new AddLocationCommand(10, 20);

            locationRepoMock.Setup(r => r.GetByCoordinatesAsync(command.Latitude, command.Longitude))
                            .ReturnsAsync(existingLocation);

            weatherApiMock.Setup(w => w.GetTemperatureAsync(command.Latitude, command.Longitude))
                          .ReturnsAsync(15);

            forecastRepoMock.Setup(f => f.AddAsync(It.IsAny<WeatherForecast>()))
                            .ReturnsAsync((WeatherForecast wf) => wf);

            var handler = new AddLocationCommandHandler(
                locationRepoMock.Object,
                forecastRepoMock.Object,
                weatherApiMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(existingLocation.Id, result.LocationId);
            Assert.Equal(15, result.TemperatureC);

            locationRepoMock.Verify(r => r.AddAsync(It.IsAny<Location>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_SetTemperatureToZero_WhenWeatherApiFails()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();
            var forecastRepoMock = new Mock<IWeatherForecastRepository>();
            var weatherApiMock = new Mock<IWeatherApiService>();

            var command = new AddLocationCommand(10, 20);

            locationRepoMock.Setup(r => r.GetByCoordinatesAsync(command.Latitude, command.Longitude))
                            .ReturnsAsync((Location)null);

            locationRepoMock.Setup(r => r.AddAsync(It.IsAny<Location>()))
                            .ReturnsAsync((Location loc) => { loc.Id = 1; return loc; });

            weatherApiMock.Setup(w => w.GetTemperatureAsync(command.Latitude, command.Longitude))
                          .ThrowsAsync(new Exception("API failed"));

            forecastRepoMock.Setup(f => f.AddAsync(It.IsAny<WeatherForecast>()))
                            .ReturnsAsync((WeatherForecast wf) => wf);

            var handler = new AddLocationCommandHandler(
                locationRepoMock.Object,
                forecastRepoMock.Object,
                weatherApiMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.TemperatureC);
        }

        [Fact]
        public async Task Handle_Should_ThrowInvalidOperationException_WhenAddingLocationFails()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();
            var forecastRepoMock = new Mock<IWeatherForecastRepository>();
            var weatherApiMock = new Mock<IWeatherApiService>();

            var command = new AddLocationCommand(10, 20);

            locationRepoMock.Setup(r => r.GetByCoordinatesAsync(command.Latitude, command.Longitude))
                            .ReturnsAsync((Location)null);

            locationRepoMock.Setup(r => r.AddAsync(It.IsAny<Location>()))
                            .ThrowsAsync(new Exception("DB error"));

            var handler = new AddLocationCommandHandler(
                locationRepoMock.Object,
                forecastRepoMock.Object,
                weatherApiMock.Object
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}