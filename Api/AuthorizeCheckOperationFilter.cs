using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Api
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        //public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //{
        //    //var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
        //    //    .Union(context.MethodInfo.GetCustomAttributes(true))
        //    //    .OfType<AuthorizeAttribute>().Any();

        //    //if (hasAuthorize)
        //    //{
        //    //    //operation.Responses.Add("401", new Response { Description = "Unauthorized" });
        //    //    //operation.Responses.Add("403", new Response { Description = "Forbidden" });

        //    //    //operation.Security = new List<IDictionary<string, IEnumerable<string>>> {
        //    //    //    new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] { "api1" } }}
        //    //    //};
        //    //}
        //}

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterDescriptors.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterDescriptors.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<OpenApiParameter>();
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "access token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        //Default = new OpenApiString("Bearer {access token}"),
                    }
                });

                var auth = new Dictionary<string, IEnumerable<string>> { { "oauth2", new[] { "api1" } } };

                //operation.Security = new List<IDictionary<string, IEnumerable<string>>>();

            }
        }
    }
}
