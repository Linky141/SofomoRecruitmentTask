using Moq;
using WeatherService.Application.Features.WeatherForecasts.Queries.GetAllLocations;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;

namespace WeatherService.Tests.Handlers
{
    public class GetAllLocationsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_ReturnListOfLocationDtos_WhenLocationsExist()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();

            var locations = new List<Location>
            {
                new Location { Id = 1, Latitude = 10, Longitude = 20 },
                new Location { Id = 2, Latitude = 30, Longitude = 40 }
            };

            locationRepoMock.Setup(r => r.GetAllAsync())
                            .ReturnsAsync(locations);

            var handler = new GetAllLocationsQueryHandler(locationRepoMock.Object);

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

            locationRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenNoLocationsExist()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();

            locationRepoMock.Setup(r => r.GetAllAsync())
                            .ReturnsAsync(new List<Location>());

            var handler = new GetAllLocationsQueryHandler(locationRepoMock.Object);

            // Act
            var result = await handler.Handle(new GetAllLocationsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            locationRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_CallRepositoryMethodOnce()
        {
            // Arrange
            var locationRepoMock = new Mock<ILocationRepository>();

            locationRepoMock.Setup(r => r.GetAllAsync())
                            .ReturnsAsync(new List<Location>());

            var handler = new GetAllLocationsQueryHandler(locationRepoMock.Object);

            // Act
            await handler.Handle(new GetAllLocationsQuery(), CancellationToken.None);

            // Assert
            locationRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}