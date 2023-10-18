using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Api.Web;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddApplicationApi(this IMvcBuilder mvc)
    {
        mvc.AddApplicationPart(typeof(ServiceCollectionExtensions).Assembly)
            .AddControllersAsServices()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
            });
        return mvc;
    }

    public static void MapApiControllerRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: Constants.RouteName,
            pattern: Constants.RoutePrefix + "/{controller}/{id?}");
    }

    public static void AddApiPolicies(this AuthorizationOptions options)
    {
        // TODO: Add policies here
        // options.AddPolicy(DemoPolicyName, policy =>
        // {
        //    policy.RequireAuthenticatedUser();
        //    policy.RequireClaim("permissions", "demo");
        //});
    }
}