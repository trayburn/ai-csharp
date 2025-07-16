using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherDemo.Library;
using System.Collections.Generic;

namespace WeatherDemo.Web.Pages
{
    public class WeatherModel : PageModel
    {
        public List<WeatherReport> Reports { get; set; }

        public void OnGet()
        {
            var service = new WeatherService();
            Reports = service.GetRandomWeatherReports();
        }
    }
}
