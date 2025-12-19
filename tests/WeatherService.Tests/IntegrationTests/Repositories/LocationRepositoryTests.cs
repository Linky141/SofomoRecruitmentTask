using Microsoft.EntityFrameworkCore;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Domain.Entities;

namespace WeatherService.Tests.Repositories
{
    public class LocationRepositoryTests
    {
        private WeatherDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new WeatherDbContext(options);
        }

        [Fact]
        public async Task AddAsync_Should_AddLocation_And_AssignId()
        {
            // Arrange
            var context = CreateDbContext(nameof(AddAsync_Should_AddLocation_And_AssignId));
            var repo = new LocationRepository(context);

            var location = new Location { Latitude = 10, Longitude = 20 };

            // Act
            var added = await repo.AddAsync(location);

            // Assert
            Assert.NotNull(added);
            Assert.True(added.Id > 0);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnLocation_WhenExists()
        {
            // Arrange
            var context = CreateDbContext(nameof(GetByIdAsync_Should_ReturnLocation_WhenExists));
            var repo = new LocationRepository(context);

            var loc = new Location { Latitude = 10, Longitude = 20 };
            context.Locations.Add(loc);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetByIdAsync(loc.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loc.Id, result!.Id);
            Assert.Equal(10, result.Latitude);
            Assert.Equal(20, result.Longitude);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNull_WhenNotFound()
        {
            var context = CreateDbContext(nameof(GetByIdAsync_Should_ReturnNull_WhenNotFound));
            var repo = new LocationRepository(context);

            var result = await repo.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByCoordinatesAsync_Should_ReturnLocation_WhenExists()
        {
            // Arrange
            var context = CreateDbContext(nameof(GetByCoordinatesAsync_Should_ReturnLocation_WhenExists));
            var repo = new LocationRepository(context);

            var loc = new Location { Latitude = 10, Longitude = 20 };
            context.Locations.Add(loc);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetByCoordinatesAsync(10, 20);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loc.Id, result!.Id);
        }

        [Fact]
        public async Task GetByCoordinatesAsync_Should_ReturnNull_WhenNotFound()
        {
            var context = CreateDbContext(nameof(GetByCoordinatesAsync_Should_ReturnNull_WhenNotFound));
            var repo = new LocationRepository(context);

            var result = await repo.GetByCoordinatesAsync(1, 1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllLocations()
        {
            // Arrange
            var context = CreateDbContext(nameof(GetAllAsync_Should_ReturnAllLocations));
            var repo = new LocationRepository(context);

            context.Locations.Add(new Location { Latitude = 10, Longitude = 20 });
            context.Locations.Add(new Location { Latitude = 30, Longitude = 40 });
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeleteAsync_Should_RemoveLocation_WhenExists()
        {
            // Arrange
            var context = CreateDbContext(nameof(DeleteAsync_Should_RemoveLocation_WhenExists));
            var repo = new LocationRepository(context);

            var loc = new Location { Latitude = 10, Longitude = 20 };
            context.Locations.Add(loc);
            await context.SaveChangesAsync();

            // Act
            await repo.DeleteAsync(loc.Id);

            // Assert
            var check = await context.Locations.FindAsync(loc.Id);
            Assert.Null(check);
        }

        [Fact]
        public async Task DeleteAsync_Should_ThrowKeyNotFound_WhenNotExists()
        {
            // Arrange
            var context = CreateDbContext(nameof(DeleteAsync_Should_ThrowKeyNotFound_WhenNotExists));
            var repo = new LocationRepository(context);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAsync(999));
        }
    }
}