using Moq;
using WeatherService.Application.Features.WeatherForecasts.Commands.DeleteLocation;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;

namespace WeatherService.Tests.Handlers
{
    public class DeleteLocationCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_DeleteLocation_WhenLocationExists()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();

            var location = new Location
            {
                Id = 5,
                Latitude = 10,
                Longitude = 20
            };

            locationRepoMock.Setup(r => r.GetByIdAsync(5))
                            .ReturnsAsync(location);

            locationRepoMock.Setup(r => r.DeleteAsync(5))
                            .Returns(Task.CompletedTask);

            var handler = new DeleteLocationCommandHandler(locationRepoMock.Object);

            var command = new DeleteLocationCommand(5);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(location.Id, result.Id);
            Assert.Equal(location.Latitude, result.Latitude);
            Assert.Equal(location.Longitude, result.Longitude);

            locationRepoMock.Verify(r => r.GetByIdAsync(5), Times.Once);
            locationRepoMock.Verify(r => r.DeleteAsync(5), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowKeyNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();

            locationRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((Location)null);

            var handler = new DeleteLocationCommandHandler(locationRepoMock.Object);

            var command = new DeleteLocationCommand(10);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            locationRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ReturnCorrectDto()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();

            var location = new Location
            {
                Id = 3,
                Latitude = 55.5,
                Longitude = 22.2
            };

            locationRepoMock.Setup(r => r.GetByIdAsync(3))
                            .ReturnsAsync(location);

            locationRepoMock.Setup(r => r.DeleteAsync(3))
                            .Returns(Task.CompletedTask);

            var handler = new DeleteLocationCommandHandler(locationRepoMock.Object);

            // Act
            var result = await handler.Handle(new DeleteLocationCommand(3), CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Id);
            Assert.Equal(55.5, result.Latitude);
            Assert.Equal(22.2, result.Longitude);
        }
    }
}