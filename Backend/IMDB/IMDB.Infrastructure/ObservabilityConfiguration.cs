using IMDB.Infrastructure.Observability.Configuration;
using IMDB.Infrastructure.Observability.Formatters;
using IMDB.Infrastructure.Observability.Models;
using IMDB.Infrastructure.Observability.Sinks.OpenSearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace IMDB.Infrastructure;

public static partial class ObservabilityConfiguration
{
    public static void AddObservability(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new ObservabilityOptions();
        services.Configure<ObservabilityOptions>(configuration.GetSection("observability"));
        configuration.Bind("observability", options);
    }

    public static IHostBuilder UseCustomLogging(this IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var tmp = configurationBuilder.Build();
        });

        builder.UseSerilog((hostingContext, loggerConfig) =>
        {

            var options = new ObservabilityOptions();
            hostingContext.Configuration.Bind("observability", options);

            // Necessary to load our abstraction
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                var tmp = configurationBuilder.Build();
                configurationBuilder.Add(new SerilogCustomConfigurationSource(tmp));
            });


            loggerConfig
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning);

            if (options?.Logging?.OpenSearchConfiguration != null)
            {
                var sink = new OpenSearchBatchingSink(options.Logging.OpenSearchConfiguration, new JsonLogFormatter());
                var batchinOptions = new PeriodicBatchingSinkOptions()
                {
                    BatchSizeLimit = options.Logging.OpenSearchConfiguration.BatchSize,
                    Period = TimeSpan.FromSeconds(options.Logging.OpenSearchConfiguration.BatchTime)
                };

                loggerConfig.WriteTo.Sink(new PeriodicBatchingSink(sink, batchinOptions));
            }


            loggerConfig.WriteTo.Console(formatter: new JsonLogFormatter());

            loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
        }, preserveStaticLogger: true);

        return builder;
    }

    public static void UseApplicationLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApplicationLoggingMiddleware>();
    }
}