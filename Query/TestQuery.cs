using HotChocolate.Authorization;
using System.Security.Claims;

namespace Mug.Query
{
    public record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    public partial class Query
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ));
        }

        [Authorize]
        public string GetTest()
        {
            return "hello";
        }

        public string GetMe(ClaimsPrincipal claimsPrincipal)
        {
            var c = claimsPrincipal;
            return "hello";
        }
    }
}
