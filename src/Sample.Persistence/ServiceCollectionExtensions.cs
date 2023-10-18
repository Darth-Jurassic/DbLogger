using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Abstractions;
using Sample.Persistence.Entities;

namespace Sample.Persistence;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationData(this IServiceCollection services, IConfiguration configuration,
        string? migrationAssembly = null)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("DefaultConnection is not configured");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, b =>
            {
                if (!string.IsNullOrWhiteSpace(migrationAssembly))
                    b.MigrationsAssembly(migrationAssembly);
            }));

        services.AddScoped<ISampleRepository, SampleRepository>();
    }
}