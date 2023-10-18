using Microsoft.Extensions.DependencyInjection;

namespace Sample.Core;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationCore(this IServiceCollection services)
    {
        services.AddScoped<ISampleManager, SampleManager>();
    }
}