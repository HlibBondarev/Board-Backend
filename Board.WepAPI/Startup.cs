using Scalar.AspNetCore;

namespace Board.WepAPI;

public static class Startup
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();





    }

    public static void Configure(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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

        app.Run();
    }
}
