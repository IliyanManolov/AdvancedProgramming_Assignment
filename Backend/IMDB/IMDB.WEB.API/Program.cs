using IMDB.Application;
using IMDB.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json.Serialization;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.UseCustomConfiguration();

        builder.Services.AddObservability(builder.Configuration);
        builder.Host.UseCustomLogging();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplicationLayer();
        builder.Services.AddDatabase(builder.Configuration);

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/keys"));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp",
                policy => policy
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );
        });


        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(settings =>
            {
                settings.LoginPath = "/oauth/login";
                settings.Cookie.IsEssential = true;
                settings.Cookie.HttpOnly = false;
                settings.SlidingExpiration = true;
                settings.ExpireTimeSpan = TimeSpan.FromHours(48);
                settings.Cookie.Name = "IMDB_Cookie";
                settings.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseApplicationLogging();

        //app.UseHttpsRedirection();

        app.UseCors("AllowReactApp");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.UseMigrations();

        app.Run();
    }
}