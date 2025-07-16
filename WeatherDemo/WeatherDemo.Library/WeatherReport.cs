using System;

namespace WeatherDemo.Library
{
    public class WeatherReport
    {
        public string? City { get; set; }
        public string? Description { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
