using Sample.Host;

Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .Build().Run();