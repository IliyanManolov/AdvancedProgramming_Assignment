using IMDB.Domain.AbstractModels;

namespace IMDB.Domain.Models;

public class Movie : Media
{
    public string? TrailerUrl { get; set; }
    public long Length { get; set; }
}