using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewProject
{
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
