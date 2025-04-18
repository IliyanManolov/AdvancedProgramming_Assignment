namespace IMDB.Infrastructure.Observability.Models;

public class LoggingConfiguration
{
    public LoggingSeverity Severity { get; set; } = LoggingSeverity.Info;
    public OpenSearchSinkOptions? OpenSearchConfiguration { get; set; }
}
