using Board.DataAccess.Repository.Base;
using Board.WepAPI.Middleware;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Exceptions;

namespace Board.WepAPI;

public static class Startup
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        // Re-configure Serilog to use the full DI container (Enrichers, Services, etc.)
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails());


        var services = builder.Services;
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        services.AddControllers();

        //services.AddScoped(typeof(IEntityRepositoryBase<,>), typeof(EntityRepositoryBase<,>));
        services.AddTransient(typeof(IEntityRepositoryBase<,>), typeof(EntityRepositoryBase<,>));
    }

    public static void Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseExceptionHandler();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthentication();

        //app.UseAuthorization();

        app.MapControllers();

        // Be sure to clear the logger when finished
        app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

        app.Run();
    }
}
