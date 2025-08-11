using System;
using System.Collections.Generic;
using System.Linq;
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
            mockRandom.SetupSequence(r => r.Next(0, 16))
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
            mockRandom.SetupSequence(r => r.Next(0, 16))
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
            mockRandom.SetupSequence(r => r.Next(0, 16))
                .Returns(0).Returns(1).Returns(2);
            mockRandom.SetupSequence(r => r.Next(0, 7))
                .Returns(0).Returns(1).Returns(2);
            mockRandom.SetupSequence(r => r.Next(-10, 35))
                .Returns(10).Returns(11).Returns(12);
            var service = new WeatherService(mockRandom.Object);

            // Act
            var reports = service.GetRandomWeatherReports(3);

            // Assert
            mockRandom.Verify(r => r.Next(0, 16), Times.AtLeastOnce());
            mockRandom.Verify(r => r.Next(0, 7), Times.AtLeastOnce());
            mockRandom.Verify(r => r.Next(-10, 35), Times.AtLeastOnce());
        }

        [Fact]
        public void WeatherService_Has_At_Least_Two_Cities_Per_Continent()
        {
            // Arrange & Act - Use reflection to access the private Cities array
            var citiesField = typeof(WeatherService).GetField("Cities", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var cities = (ValueTuple<string, string>[])citiesField.GetValue(null);

            // Group cities by continent and count them
            var continentCounts = cities
                .GroupBy(city => city.Item2)
                .ToDictionary(group => group.Key, group => group.Count());

            // Assert - Every continent should have at least 2 cities
            var expectedContinents = new[] { "North America", "Europe", "Asia", "Australia", "South America", "Africa", "Antarctica" };
            
            foreach (var continent in expectedContinents)
            {
                Assert.True(continentCounts.ContainsKey(continent), $"Missing continent: {continent}");
                Assert.True(continentCounts[continent] >= 2, $"Continent {continent} has only {continentCounts[continent]} cities, expected at least 2");
            }
        }
    }
}
