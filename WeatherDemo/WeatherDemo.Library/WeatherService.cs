using System;
using System.Collections.Generic;

namespace WeatherDemo.Library
{
    public class WeatherService
    {
        private static readonly (string Name, string Continent)[] Cities = new[]
        {
            ("New York", "North America"),
            ("London", "Europe"),
            ("Tokyo", "Asia"),
            ("Sydney", "Australia"),
            ("Paris", "Europe"),
            ("Berlin", "Europe"),
            ("Moscow", "Europe"),
            ("Rio de Janeiro", "South America"),
            ("Cape Town", "Africa"),
            ("Toronto", "North America")
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
                var cityTuple = Cities[_randomProvider.Next(0, Cities.Length)];
                if (selectedCities.Add(cityTuple.Name))
                {
                    reports.Add(new WeatherReport
                    {
                        City = cityTuple.Name,
                        Description = Descriptions[_randomProvider.Next(0, Descriptions.Length)],
                        TemperatureC = _randomProvider.Next(-10, 35)
                    });
                }
            }
            return reports;
        }

        public List<WeatherReport> GetNorthAmericanWeatherReports(int count = 2)
        {
            var northAmericanCities = new List<(string Name, string Continent)>();
            foreach (var city in Cities)
            {
                if (city.Continent == "North America")
                    northAmericanCities.Add(city);
            }
            var selectedCities = new HashSet<string>();
            var reports = new List<WeatherReport>();
            while (selectedCities.Count < count && selectedCities.Count < northAmericanCities.Count)
            {
                var cityTuple = northAmericanCities[_randomProvider.Next(0, northAmericanCities.Count)];
                if (selectedCities.Add(cityTuple.Name))
                {
                    reports.Add(new WeatherReport
                    {
                        City = cityTuple.Name,
                        Description = Descriptions[_randomProvider.Next(0, Descriptions.Length)],
                        TemperatureC = _randomProvider.Next(-10, 35)
                    });
                }
            }
            return reports;
        }
    }
}

