using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InterviewProject
{
    
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : MyBaseController
    {


        private readonly ILogger<WeatherForecastController> _logger;
        private WeatherClient weatherClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherClient client) :base(logger)
        {
            _logger = logger;
            weatherClient = client;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var items = await this.weatherClient.GetForecasts(DateTime.Now.Year, DateTime.Now.Month);
            return items;
        }

        [ApiKeyAuth]
        [HttpGet("reset")]
        public async Task Reset()
        {
            await this.weatherClient.Reset();
        }

        [ApiKeyAuth]
        [HttpPost("generateForecasts")]
        public async Task GenerateForecasts([FromBody] WeatherForecastGenerationRequest request)
        {
            await this.weatherClient.GenerateForecast(request);
        }
    }
}
