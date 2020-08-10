using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewProject
{
    public class MyHeaderFilter : IOperationFilter
    {
        //public void Apply(Operation operation, OperationFilterContext context)
        //{
        //    if (operation.Parameters == null)
        //        operation.Parameters = new List<IParameter>();

        //    operation.Parameters.Add(new NonBodyParameter
        //    {
        //        Name = "MY-HEADER",
        //        In = "header",
        //        Type = "string",
        //        Required = true // set to false if this is optional
        //    });
        //}

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributres = context.MethodInfo.GetCustomAttributes(true);
            if (attributres.Where(t => t is ApiKeyAuthAttribute).Any())
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Description = "[add api key]",
                    Schema = new OpenApiSchema
                    {
                        Type = "String",
                        Default = new OpenApiString("[Add key]")
                    }
                }); ;
            }

        }
    }
}
