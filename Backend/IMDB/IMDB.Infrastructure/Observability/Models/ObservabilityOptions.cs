namespace IMDB.Infrastructure.Observability.Models;

public class ObservabilityOptions
{
    public LoggingConfiguration Logging { get; set; } = new LoggingConfiguration();
}
