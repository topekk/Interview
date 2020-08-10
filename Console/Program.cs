using Newtonsoft.Json;
using System;

namespace Console
{
    public class Program
    {
        static WeatherForecastClient weatherForecastClient = new WeatherForecastClient("interviewprojectx.azurewebsites.net");
        //static WeatherForecastClient weatherForecastClient = new WeatherForecastClient("localhost:5001");
        public static void Main(string[] args)
        {
            //System.Console.In.Read();
            var result = weatherForecastClient.HealthCheck().Result;
            System.Console.WriteLine($"healthcheck: {result}");

            var postResponse = weatherForecastClient.GenerateForecasts(10, 12, 40);
            System.Console.WriteLine($"{JsonConvert.SerializeObject(postResponse.Result)}");

            var forecastGet = weatherForecastClient.GetWeatherForecastsAsync().Result;
            foreach(var forecast in forecastGet)
            {
                System.Console.WriteLine($"{JsonConvert.SerializeObject(forecast)}");
            }

            var resetResult = weatherForecastClient.Reset().Result;
            System.Console.WriteLine($"{JsonConvert.SerializeObject(resetResult)}");


        }
    }
}
