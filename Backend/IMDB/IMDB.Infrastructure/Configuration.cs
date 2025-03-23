using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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


        return services;
    }
}
