using System;
using System.Collections.Generic;
using Moq;
using WeatherDemo.Library;
using Xunit;

namespace WeatherDemo.Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public void GetNorthAmericanWeatherReports_Returns_Only_NorthAmerican_Cities()
        {
            // Arrange
            var mockRandom = new Mock<IRandomProvider>();
            // Assume North American cities are at indices 0 (New York) and 9 (Toronto)
            mockRandom.SetupSequence(r => r.Next(0, 2))
                .Returns(0).Returns(1);
            mockRandom.SetupSequence(r => r.Next(0, 7))
                .Returns(0).Returns(1);
            mockRandom.SetupSequence(r => r.Next(-10, 35))
                .Returns(15).Returns(20);
            var service = new WeatherService(mockRandom.Object);

            // Act
            var reports = service.GetNorthAmericanWeatherReports(2);

            // Assert
            var northAmericanCities = new HashSet<string> { "New York", "Toronto" };
            Assert.Equal(2, reports.Count);
            Assert.All(reports, r => Assert.Contains(r.City, northAmericanCities));
        }
    
        [Fact]
        public void GetRandomWeatherReports_Returns_Correct_Count()
        {
            // Arrange
            var mockRandom = new Mock<IRandomProvider>();
            mockRandom.SetupSequence(r => r.Next(0, 10))
                .Returns(0).Returns(1).Returns(2).Returns(3).Returns(4);
            mockRandom.SetupSequence(r => r.Next(0, 7))
                .Returns(0).Returns(1).Returns(2).Returns(3).Returns(4);
            mockRandom.SetupSequence(r => r.Next(-10, 35))
                .Returns(20).Returns(21).Returns(22).Returns(23).Returns(24);
            var service = new WeatherService(mockRandom.Object);

            // Act
            var reports = service.GetRandomWeatherReports(5);

            // Assert
            Assert.Equal(5, reports.Count);
            Assert.All(reports, r => Assert.False(string.IsNullOrEmpty(r.City)));
            Assert.All(reports, r => Assert.False(string.IsNullOrEmpty(r.Description)));
        }

        [Fact]
        public void GetRandomWeatherReports_Cities_Are_Unique()
        {
            // Arrange
            var mockRandom = new Mock<IRandomProvider>();
            mockRandom.SetupSequence(r => r.Next(0, 10))
                .Returns(0).Returns(1).Returns(2).Returns(3).Returns(4);
            mockRandom.Setup(r => r.Next(0, 7)).Returns(0);
            mockRandom.Setup(r => r.Next(-10, 35)).Returns(25);
            var service = new WeatherService(mockRandom.Object);

            // Act
            var reports = service.GetRandomWeatherReports(5);

            // Assert
            var cities = new HashSet<string>(reports.ConvertAll(r => r.City));
            Assert.Equal(5, cities.Count);
        }

        [Fact]
        public void GetRandomWeatherReports_Uses_RandomProvider()
        {
            // Arrange
            var mockRandom = new Mock<IRandomProvider>();
            // Ensure unique cities are selected
            mockRandom.SetupSequence(r => r.Next(0, 10))
                .Returns(0).Returns(1).Returns(2);
            mockRandom.SetupSequence(r => r.Next(0, 7))
                .Returns(0).Returns(1).Returns(2);
            mockRandom.SetupSequence(r => r.Next(-10, 35))
                .Returns(10).Returns(11).Returns(12);
            var service = new WeatherService(mockRandom.Object);

            // Act
            var reports = service.GetRandomWeatherReports(3);

            // Assert
            mockRandom.Verify(r => r.Next(0, 10), Times.AtLeastOnce());
            mockRandom.Verify(r => r.Next(0, 7), Times.AtLeastOnce());
            mockRandom.Verify(r => r.Next(-10, 35), Times.AtLeastOnce());
        }
    }
}
