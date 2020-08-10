using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewProject
{
    public class WeatherClient
    {
        public const string Partition = "/yearMonth";
        public static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private ILogger logger;
        private IDatabaseClient<WeatherForecast> client;

        public WeatherClient(ILogger<WeatherClient> logger, ILogger<DatabaseClient<WeatherForecast>> weatherForecastClientLogger, DatabaseClientConfiguration config)
        :  this(logger, new DatabaseClient<WeatherForecast>(weatherForecastClientLogger, config, nameof(WeatherForecast), Partition))
        {

        }

        public WeatherClient(ILogger<WeatherClient> logger, IDatabaseClient<WeatherForecast> client)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task Init()
        {
            await this.client.Init();
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecasts(int year, int month)
        {
            await Init();
            return await this.client.ExecuteQuery($"Select * FROM c WHERE c.yearMonth = '{year}-{month}'");
        }

        public async Task GenerateForecast(WeatherForecastGenerationRequest request)
        {
            await Init();
            var rng = new Random();
            var forecasts = Enumerable.Range(1, request.Count).Select(index => new WeatherForecast
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(request.MinTemperature, request.MaxTemperature),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }); ;

            foreach(var forecast in forecasts)
            {
                await client.UpsertItem(forecast.YearMonth, forecast);
            }
        }

        public async Task Reset()
        {
            await this.client.Reset();
        }


    }

    public class WeatherClientConfig
    {

    }
}
