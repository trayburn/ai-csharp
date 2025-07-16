using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherDemo.Library;
using System.Collections.Generic;

namespace WeatherDemo.Web.Pages
{
    public class WeatherModel : PageModel
    {
        public List<WeatherReport> Reports { get; set; }

        private readonly WeatherService _weatherService;

        public WeatherModel(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public void OnGet()
        {
            Reports = _weatherService.GetRandomWeatherReports();
        }
    }
}
