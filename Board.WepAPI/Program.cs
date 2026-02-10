using Board.WepAPI;
using Serilog;

// 1. Setup early logging (Bootstrap Logger) using configuration files
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();

try
{
    Log.Information("Application starting up...");

    var builder = WebApplication.CreateBuilder(args);

    // 2. Register application services via extension method
    builder.AddApplicationServices();

    var app = builder.Build();

    // 3. Setup middleware pipeline via extension method
    app.Configure();

    Log.Information("Application has started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly during startup");
}
finally
{
    Log.Information("Application shut down complete");
    // Ensure all logs are written to the file before closing
    Log.CloseAndFlush();
}