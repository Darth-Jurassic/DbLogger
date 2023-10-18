using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Persistence;
using Sample.Persistence.Entities;

namespace Sample.Migrator;

public class ApplyMigrationsService : IHostedService
{
    private readonly IHost _host;
    private readonly IServiceProvider _services;
    private IServiceScope? _scope;
    private ApplicationDbContext? _db;

    public ApplyMigrationsService(IHost host, IServiceProvider services)
    {
        _host = host;
        _services = services;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _scope = _services.CreateScope();
        _db = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await _db.Database.MigrateAsync(cancellationToken);
        }
        finally
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CA2016
            // ReSharper disable once MethodSupportsCancellation
            _host.StopAsync();
#pragma warning restore CA2016
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _scope?.Dispose();
        return Task.CompletedTask;
    }
}