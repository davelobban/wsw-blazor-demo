using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Ui.Data
{
    public class SlowWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate, int delayInSeconds)
        {
            using var client = new HttpClient();
            var resp = await client.GetAsync($"https://localhost:44318/Slow?delayInSecondds={delayInSeconds}");
            return await resp.Content.ReadFromJsonAsync<WeatherForecast[]>();
        }
    }
}
