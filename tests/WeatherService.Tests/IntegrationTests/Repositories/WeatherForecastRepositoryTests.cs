using Microsoft.EntityFrameworkCore;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Infrastructure.Repositories;

namespace WeatherService.Tests.IntegrationTests.Repositories
{
    public class WeatherForecastRepositoryTests
    {
        private WeatherDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new WeatherDbContext(options);
        }

        [Fact]
        public async Task AddAsync_Should_AddForecastToDatabase()
        {
            // Arrange
            var db = CreateDbContext(nameof(AddAsync_Should_AddForecastToDatabase));
            var repo = new WeatherForecastRepository(db);

            var forecast = new WeatherForecast
            {
                LocationId = 1,
                TemperatureC = 15,
                ForecastDate = DateTime.UtcNow
            };

            // Act
            var result = await repo.AddAsync(forecast);
            var saved = await db.WeatherForecasts.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(saved);
            Assert.Equal(forecast.LocationId, saved.LocationId);
            Assert.Equal(forecast.TemperatureC, saved.TemperatureC);
            Assert.Equal(forecast.ForecastDate, saved.ForecastDate);
        }

        [Fact]
        public async Task GetLatestByLocationIdAsync_Should_ReturnLatestForecast()
        {
            // Arrange
            var db = CreateDbContext(nameof(GetLatestByLocationIdAsync_Should_ReturnLatestForecast));
            var repo = new WeatherForecastRepository(db);

            db.WeatherForecasts.AddRange(
                new WeatherForecast { LocationId = 1, TemperatureC = 10, ForecastDate = new DateTime(2024, 1, 1) },
                new WeatherForecast { LocationId = 1, TemperatureC = 20, ForecastDate = new DateTime(2024, 2, 1) },
                new WeatherForecast { LocationId = 1, TemperatureC = 30, ForecastDate = new DateTime(2024, 3, 1) }
            );
            await db.SaveChangesAsync();

            // Act
            var result = await repo.GetLatestByLocationIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(30, result.TemperatureC);
            Assert.Equal(new DateTime(2024, 3, 1), result.ForecastDate);
        }

        [Fact]
        public async Task GetLatestByLocationIdAsync_Should_ReturnNull_WhenNoForecastsExist()
        {
            // Arrange
            var db = CreateDbContext(nameof(GetLatestByLocationIdAsync_Should_ReturnNull_WhenNoForecastsExist));
            var repo = new WeatherForecastRepository(db);

            // Act
            var result = await repo.GetLatestByLocationIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetLatestByLocationIdAsync_Should_ReturnNull_WhenLocationHasNoForecasts()
        {
            // Arrange
            var db = CreateDbContext(nameof(GetLatestByLocationIdAsync_Should_ReturnNull_WhenLocationHasNoForecasts));
            var repo = new WeatherForecastRepository(db);

            db.WeatherForecasts.Add(new WeatherForecast
            {
                LocationId = 1,
                TemperatureC = 15,
                ForecastDate = DateTime.UtcNow
            });

            await db.SaveChangesAsync();

            // Act
            var result = await repo.GetLatestByLocationIdAsync(2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_Should_AssignIdAutomatically()
        {
            // Arrange
            var db = CreateDbContext(nameof(AddAsync_Should_AssignIdAutomatically));
            var repo = new WeatherForecastRepository(db);

            var forecast = new WeatherForecast
            {
                LocationId = 1,
                TemperatureC = 99,
                ForecastDate = DateTime.UtcNow
            };

            // Act
            var saved = await repo.AddAsync(forecast);

            // Assert
            Assert.True(saved.Id > 0);
        }
    }
}