using IMDB.Application.Abstractions.Repositories;
using IMDB.Infrastructure.Database;
using IMDB.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace IMDB.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddDatabase(
    this IServiceCollection services,
    IConfiguration configuration)
    {

        var section = configuration.GetSection(nameof(DatabaseSettings));

        services.Configure<DatabaseSettings>(section);

        services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));


        services.AddDbContext<DatabaseContext>((provider, options) =>
        {
            var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>();

            options.UseSqlServer(settings.Value.ConnectionString, config =>
            {
                config.MigrationsHistoryTable(settings.Value.MigrationTable);

                config.MigrationsAssembly(typeof(DatabaseContext)
                    .Assembly.GetName()
                    .Name);
            });
        });

        services.AddRepositories();
        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWatchListRepository, WatchListRepository>();

        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IDirectorRepository, DirectorRepository>();

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IEpisodeRepository, EpisodeRepository>();
        services.AddScoped<ITvShowRepository, TvShowRepository>();

    }

    public static IHost UseMigrations(this IHost app)
    {
        var settings = app.Services
            .GetRequiredService<IOptions<DatabaseSettings>>();

        if (settings.Value.EnableMigrations)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<DatabaseContext>();

                context.Database.Migrate();
            }
        }

        return app;
    }
}
