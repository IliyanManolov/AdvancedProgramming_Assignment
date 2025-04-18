namespace IMDB.Infrastructure.Observability.Models;

public class OpenSearchSinkOptions
{
    public long BatchTime { get; set; } = 60;
    public int BatchSize { get; set; } = 50;

    public string Index { get; set; }
    public string ConnectionUrl { get; set; }
    public string Password { get; set; }

    public string User { get; set; }
}
