using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        //private readonly AdminApiConfiguration _adminApiConfiguration;

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>().Any();


            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                //operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                //{
                //    //new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] { "api1" }}}
                //};

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                   new OpenApiSecurityRequirement
                   {

                   }
                };
            }
        }
    }
}