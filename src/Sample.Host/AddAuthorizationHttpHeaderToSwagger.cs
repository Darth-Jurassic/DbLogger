using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sample.Host;

public class AddAuthorizationHttpHeaderToSwagger : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is AuthorizeAttribute))
            return;
        
        operation.Parameters ??= new List<OpenApiParameter>();

        var parameter = new OpenApiParameter
        {
            Description = "The authorization token",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        };

        if (context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
        {
            parameter.Required = false;
        }

        operation.Parameters.Add(parameter);
    }
}