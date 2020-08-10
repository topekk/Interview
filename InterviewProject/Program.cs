using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace InterviewProject
{
    public class Program
    {
        private readonly static Uri SwaggerCompanyUrl = new Uri("http://www.brexinterview.com");
        private const string SwaggerContactEmail = "support@brexinterview.com";
        private const string SwaggerCompanyName = "© brexprep 2019";
        private const string SwaggerDescription = "A Brex Interview Service.";
        private const string SwaggerVersion = "v1";
        private const string SwaggerTitle = "Brex Interview Service.";


        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
             .ConfigureServices(services =>
             {
                 services.AddSwaggerGen(c =>
                 {
                     c.SwaggerDoc(
                         SwaggerVersion,
                                     new OpenApiInfo
                                     {
                                         Title = SwaggerTitle,
                                         Version = SwaggerVersion,
                                         Description = SwaggerDescription,
                                         Contact = new OpenApiContact
                                         {
                                             Name = SwaggerCompanyName,
                                             Email = SwaggerContactEmail,
                                             Url = SwaggerCompanyUrl
                                         },
                                         License = new OpenApiLicense
                                         {
                                             Name = "Use under LICX",
                                             Url = new Uri("https://example.com/license"),
                                         }
                                     });
                     c.OperationFilter<MyHeaderFilter>();
                 });
                 services.AddSwaggerDocument();


             })

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
