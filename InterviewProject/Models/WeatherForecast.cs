using Newtonsoft.Json;
using System;

namespace InterviewProject
{
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
}
