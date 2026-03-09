using Board.BusinessLogic.Features.ForBoards.Queries;
using Board.Common.Services;
using Board.Common.Services.Api;
using Board.DataAccess.Repository;
using Board.DataAccess.Repository.Api;
using Board.WepAPI.Authorization;
using Board.WepAPI.Filters;
using Board.WepAPI.Middleware;
using DbUp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            // Disable the default automatic 400 response
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString, null)
            .WithScriptsEmbeddedInAssembly(typeof(IUserRepository).Assembly)
            .WithTransaction()
            .LogToConsole()
            .Build();

        if (upgrader.IsUpgradeRequired())
        {
            upgrader.PerformUpgrade();
        }

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetBoardsByUserIdQuery).Assembly));

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
        services.AddAuthorizationBuilder()
            .AddPolicy("MustBeMemberOfBoard", policy =>
                policy.Requirements.Add(new MustBeMemberOfBoardRequirement()))
            .AddPolicy("MustBeBoardAdmin", policy =>
                policy.Requirements.Add(new MustBeBoardAdminRequirement()))
            .AddPolicy("MustBeIssueCreatorOrAssigneeOrAdmin", policy =>
                policy.Requirements.Add(new MustBeIssueCreatorOrAssigneeOrAdminRequirement()))
            .AddPolicy("MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNull", policy =>
                policy.Requirements.Add(new MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNullRequirement()));

        services.AddScoped<IAuthorizationHandler, MustBeMemberOfBoardHandler>();
        services.AddScoped<IAuthorizationHandler, MustBeBoardAdminHandler>();
        services.AddScoped<IAuthorizationHandler, MustBeIssueCreatorOrAssigneeOrAdminHandler>();
        services.AddScoped<IAuthorizationHandler, MustBeIssueAssigneeOrAdminOrIssueAssigneeIsNullHandler>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, BoardAppAuthorizationResultHandler>();

        services.AddHttpClient<ICurrentUserService, CurrentUserService>(client =>
        {
            var authority = configuration["Auth0:Authority"];
            _ = authority ?? throw new ArgumentNullException(nameof(authority));
            client.BaseAddress = new Uri($"{authority.TrimEnd('/')}/");
        });

        //services.AddScoped(typeof(IEntityRepositoryBase<,>), typeof(EntityRepositoryBase<,>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IColumnRepository, ColumnRepository>();
        services.AddScoped<IIssueRepository, IssueRepository>();
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
