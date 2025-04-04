using IMDB.Application;
using IMDB.Infrastructure;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add this as env variable

        builder.Configuration.UseCustomConfiguration();
        // Add services to the container.

        builder.Services.AddObservability(builder.Configuration);
        builder.Host.UseCustomLogging();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplicationLayer();
        builder.Services.AddDatabase(builder.Configuration);


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp",
                policy => policy
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
        });

        //var context = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        //context.Database.Migrate();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();
        //}

        app.UseApplicationLogging();

        //app.UseHttpsRedirection();

        app.UseCors("AllowReactApp");

        app.UseAuthorization();

        app.MapControllers();

        app.UseMigrations();

        app.Run();
    }
}