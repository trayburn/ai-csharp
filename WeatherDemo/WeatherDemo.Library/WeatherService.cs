using System;
using System.Collections.Generic;

namespace WeatherDemo.Library
{
    public class WeatherService
    {
        private static readonly string[] Cities = new[]
        {
            "New York", "London", "Tokyo", "Sydney", "Paris", "Berlin", "Moscow", "Rio de Janeiro", "Cape Town", "Toronto"
        };

        private static readonly string[] Descriptions = new[]
        {
            "Sunny", "Cloudy", "Rainy", "Windy", "Stormy", "Foggy", "Snowy"
        };

        private readonly IRandomProvider _randomProvider;

        public WeatherService(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public List<WeatherReport> GetRandomWeatherReports(int count = 5)
        {
            var selectedCities = new HashSet<string>();
            var reports = new List<WeatherReport>();
            while (selectedCities.Count < count)
            {
                var city = Cities[_randomProvider.Next(0, Cities.Length)];
                if (selectedCities.Add(city))
                {
                    reports.Add(new WeatherReport
                    {
                        City = city,
                        Description = Descriptions[_randomProvider.Next(0, Descriptions.Length)],
                        TemperatureC = _randomProvider.Next(-10, 35)
                    });
                }
            }
            return reports;
        }
    }
}

