using Board.BusinessLogic.Features;
using Board.BusinessLogic.Services;
using Board.BusinessLogic.Services.Api;
using Board.DataAccess.Repository.Base;
using Board.WepAPI.Authorization;
using Board.WepAPI.Middleware;
using DbUp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        EnsureDatabase.For.SqlDatabase(connectionString);

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString, null)
            .WithScriptsEmbeddedInAssembly(typeof(EntityRepositoryBase<,>).Assembly)
            .WithTransaction()
            .LogToConsole()
            .Build();

        if (upgrader.IsUpgradeRequired())
        {
            upgrader.PerformUpgrade();
        }

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SqlStatements).Assembly));

        services.AddControllers();

        services.AddCors(options => options.AddPolicy("AllowReactApp", builder =>
            builder.AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithOrigins("http://localhost:5173")
                   .AllowCredentials()));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = configuration["Auth0:Authority"];
            options.Audience = configuration["Auth0:Audience"];
        });

        services.AddHttpClient();
        services.AddAuthorization(options =>
          options.AddPolicy("MustBeThisUser", policy =>
            policy.Requirements
              .Add(new MustBeThisUserRequirement())));

        services.AddScoped<IAuthorizationHandler, MustBeThisUserHandler>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddHttpClient<ICurrentUser, CurrentUser>(client =>
        {
            var authority = configuration["Auth:Authority"];
            _ = authority ?? throw new ArgumentNullException(nameof(authority));
            client.BaseAddress = new Uri($"{authority.TrimEnd('/')}/");
        });

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

        app.UseRouting();

        app.UseCors("AllowReactApp");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        // Be sure to clear the logger when finished
        app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

        app.Run();
    }
}
