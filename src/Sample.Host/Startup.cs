using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Sample.Api.Web;
using Sample.Core;
using Sample.Persistence;
using ServiceCollectionExtensions = Sample.Api.Web.ServiceCollectionExtensions;

namespace Sample.Host;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add domain modules.
        services.AddApplicationCore();

        // Add database.
        services.AddApplicationData(_configuration);
       
        services.AddControllersWithViews()
            .AddApplicationApi()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
        services.AddRazorPages();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            switch (Environment.GetEnvironmentVariable("ASPNETCORE_OPENAPI_AUTHORIZATION"))
            {
                case "none":
                    break;
                default:
                    c.OperationFilter<AddAuthorizationHttpHeaderToSwagger>();
                    break;
            }
            c.SwaggerDoc("v1",
                new OpenApiInfo { Version = "v1", Title = $"{typeof(Startup).FullName!.Split('.', 2)[0]} API" });

            var apiXmlFilename =
                Path.ChangeExtension(typeof(ServiceCollectionExtensions).Assembly.Location, ".xml");
            c.IncludeXmlComments(apiXmlFilename);
        });

        services.AddCors(o =>
        {
            o.AddPolicy("AllowDevLocal", builder =>
            {
                builder.WithOrigins("https://dev.local:44407")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCors("AllowDevLocal");

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapApiControllerRoutes();
            endpoints.MapRazorPages();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}
