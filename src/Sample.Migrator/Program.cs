// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Migrator;
using Sample.Persistence;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationData(hostContext.Configuration, Assembly.GetExecutingAssembly().FullName);
        services.AddHostedService<ApplyMigrationsService>();
    })
    .Build().Run();