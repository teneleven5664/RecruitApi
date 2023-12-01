using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitApi.Data;
using RecruitApi.Models;
using RecruitApi.Repository;

namespace RecruitApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRecruitRepository _recruitRepo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext db, IRecruitRepository recruitRepo)
        {
            _logger = logger;
            _recruitRepo = recruitRepo;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            var z = await _recruitRepo.GetAllAsync<ProdGroup>(x => x.Name.Contains("01"));

            foreach (var item in z)
            {
                Console.WriteLine($"Group {item.Name}");
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
