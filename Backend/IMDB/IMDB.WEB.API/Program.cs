using IMDB.Application;
using IMDB.Infrastructure;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add this as env variable
        builder.Configuration.AddJsonFile("./Config/appsettings.json", optional: true, reloadOnChange: true);
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplicationLayer();
        builder.Services.AddDatabase(builder.Configuration);

        //var context = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        //context.Database.Migrate();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseMigrations();

        app.Run();
    }
}