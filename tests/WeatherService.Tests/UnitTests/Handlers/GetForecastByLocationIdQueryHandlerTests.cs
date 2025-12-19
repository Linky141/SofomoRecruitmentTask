using Moq;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Application.Features.WeatherForecasts.ueries.GetForecastByLocationId;

namespace WeatherService.Tests.Handlers
{
    public class GetForecastByLocationIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnForecastResponseDto_WhenLocationAndForecastExist()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();
            var forecastRepoMock = new Mock<IWeatherForecastRepository>();

            var location = new Location { Id = 1, Latitude = 50, Longitude = 20 };
            var forecast = new WeatherForecast
            {
                LocationId = 1,
                TemperatureC = 15,
                ForecastDate = new DateTime(2024, 1, 1)
            };

            locationRepoMock.Setup(r => r.GetByIdAsync(1))
                            .ReturnsAsync(location);

            forecastRepoMock.Setup(r => r.GetLatestByLocationIdAsync(1))
                            .ReturnsAsync(forecast);

            var handler = new GetForecastByLocationIdQueryHandler(
                forecastRepoMock.Object,
                locationRepoMock.Object
            );

            // Act
            var result = await handler.Handle(new GetForecastByLocationIdQuery(1), CancellationToken.None);

            // Assert
            Assert.Equal(1, result.LocationId);
            Assert.Equal(50, result.Latitude);
            Assert.Equal(20, result.Longitude);
            Assert.Equal(15, result.TemperatureC);
            Assert.Equal(new DateTime(2024, 1, 1), result.ForecastDate);

            locationRepoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            forecastRepoMock.Verify(r => r.GetLatestByLocationIdAsync(1), Times.Once);
        }


        [Fact]
        public async Task Handle_Should_ThrowKeyNotFoundException_WhenLocationDoesNotExist()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();
            var forecastRepoMock = new Mock<IWeatherForecastRepository>();

            locationRepoMock.Setup(r => r.GetByIdAsync(1))
                            .ReturnsAsync((Location)null);

            var handler = new GetForecastByLocationIdQueryHandler(
              forecastRepoMock.Object,
                locationRepoMock.Object
            );

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(new GetForecastByLocationIdQuery(1), CancellationToken.None)
            );

            locationRepoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            forecastRepoMock.Verify(r => r.GetLatestByLocationIdAsync(It.IsAny<int>()), Times.Never);
        }


        [Fact]
        public async Task Handle_Should_ThrowKeyNotFoundException_WhenForecastDoesNotExist()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();
            var forecastRepoMock = new Mock<IWeatherForecastRepository>();

            var location = new Location { Id = 1, Latitude = 50, Longitude = 20 };

            locationRepoMock.Setup(r => r.GetByIdAsync(1))
                            .ReturnsAsync(location);

            forecastRepoMock.Setup(r => r.GetLatestByLocationIdAsync(1))
                            .ReturnsAsync((WeatherForecast)null);

            var handler = new GetForecastByLocationIdQueryHandler(
                forecastRepoMock.Object,
                locationRepoMock.Object
            );

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(new GetForecastByLocationIdQuery(1), CancellationToken.None)
            );

            locationRepoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
            forecastRepoMock.Verify(r => r.GetLatestByLocationIdAsync(1), Times.Once);
        }
    }
}