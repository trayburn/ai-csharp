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

        public List<WeatherReport> GetRandomWeatherReports(int count = 5)
        {
            var rng = new Random();
            var selectedCities = new HashSet<string>();
            var reports = new List<WeatherReport>();
            while (selectedCities.Count < count)
            {
                var city = Cities[rng.Next(Cities.Length)];
                if (selectedCities.Add(city))
                {
                    reports.Add(new WeatherReport
                    {
                        City = city,
                        Description = Descriptions[rng.Next(Descriptions.Length)],
                        TemperatureC = rng.Next(-10, 35)
                    });
                }
            }
            return reports;
        }
    }
}

