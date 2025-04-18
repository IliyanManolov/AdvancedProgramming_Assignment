namespace IMDB.Infrastructure.Observability.Models.DTOs;

public record ExModel
{
    public string st { get; set; }
    public string err { get; set; }
    public string errType { get; set; }
}
