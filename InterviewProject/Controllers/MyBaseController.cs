using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InterviewProject
{
    public abstract class MyBaseController : ControllerBase
    {
        public static TimeSpan Uptime
        {
            get
            {
                var start = startTime;
                var now = DateTime.UtcNow;
                return now.Subtract(start);
            }
        }
        private static readonly DateTime startTime = DateTime.UtcNow;

        protected ILogger logger;

        public MyBaseController(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// verify service is alive.
        /// </summary>
        /// <returns>ping if ok.</returns>
        [HttpGet]
        [Route("healthcheck")]
#pragma warning disable CA1822 // Mark members as static
        public ActionResult<string> HealthCheck() => $"Uptime: {Uptime}";



        /// <summary>
        /// Sets the header values.
        /// </summary>
        /// <param name="headerValues">The header values.</param>
        protected void SetHeaderValues(object headerValues)
        {
            System.Diagnostics.Contracts.Contract.Requires(headerValues != null);
            var jsonValues = Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(headerValues));
            foreach (var token in jsonValues)
            {
                string headerValue;
                if (!string.IsNullOrWhiteSpace(jsonValues[token.Key]?.Value<string>()))
                {
                    headerValue = jsonValues[token.Key].Value<string>();
                    this.Response.Headers.Add(
                               token.Key,
                               new StringValues(headerValue));
                }

            }
        }
    }
}