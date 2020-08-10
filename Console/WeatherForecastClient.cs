using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    public class WeatherForecastClient
    {
        private string baseEndpoint;
        private HttpClient client;

        public WeatherForecastClient(string baseEndpoint)
        {
            this.baseEndpoint = baseEndpoint;
            this.client = new HttpClient();
        }



        public async Task<HttpResponseMessage> GenerateForecasts(int count, int minTemp, int maxTemp)
        {

            var requestUri = new Uri($"https://{this.baseEndpoint}/WeatherForecast/generateForecasts");
            var weatherForecastGenerationRequest = new WeatherForecastGenerationRequest()
            {
                Count = count,
                MinTemperature = minTemp,
                MaxTemperature = maxTemp
            };



            using (var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = requestUri,
                Headers = {
                    {"ApiKey", "strawberryfieldsforever" }
                }
            })
            {
                HttpContent httpContent = null;


                var ms = new MemoryStream();
                SerializeJsonIntoStream(weatherForecastGenerationRequest, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpRequestMessage.Content = httpContent;


                var response = await client.SendAsync(httpRequestMessage);
                return response;
            }

                
        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }


        public async Task<List<WeatherForecast>> GetWeatherForecastsAsync()
        {

            var requestUri = new Uri($"https://{this.baseEndpoint}/WeatherForecast");

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri,
                Headers = {
                    //{ HttpRequestHeader.Authorization.ToString(), "Bearer xxxxxxxxxxxxxxxxxxxx" },
                    { HttpRequestHeader.Accept.ToString(), "application/json" },
                    { "X-Version", "1" }
                },
            };

            var response = await client.SendAsync(httpRequestMessage);
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(await response.Content.ReadAsStreamAsync()))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<List<WeatherForecast>>(jsonTextReader);
            }
        }


        public async Task<string> HealthCheck()
        {

            var requestUri = new Uri($"https://{this.baseEndpoint}/WeatherForecast/healthcheck");

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri,
                Headers = {
                    //{ HttpRequestHeader.Authorization.ToString(), "Bearer xxxxxxxxxxxxxxxxxxxx" },
                    { HttpRequestHeader.Accept.ToString(), "application/json" },
                    { "X-Version", "1" }
                },
            };

            var response = await client.SendAsync(httpRequestMessage);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> Reset()
        {

            var requestUri = new Uri($"https://{this.baseEndpoint}/WeatherForecast/reset");

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri,
                Headers = {
                    {"ApiKey", "strawberryfieldsforever" },
                    { HttpRequestHeader.Accept.ToString(), "application/json" },
                    { "X-Version", "1" }
                },
            };

            var response = await client.SendAsync(httpRequestMessage);
            return response;
        }

    }

    public class WeatherForecast
    {
        [JsonProperty("id")]
        public string Id { get; set; }

            [JsonProperty("yearMonth")]
        public string YearMonth => $"{Date.Year}-{Date.Month}";

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonIgnore]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    public class WeatherForecastGenerationRequest
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("minTemperature")]
        public int MinTemperature { get; set; }

        [JsonProperty("maxTemperature")]
        public int MaxTemperature { get; set; }
    }
}
