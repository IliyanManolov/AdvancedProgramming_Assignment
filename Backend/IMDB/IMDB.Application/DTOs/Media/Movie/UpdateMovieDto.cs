namespace IMDB.Application.DTOs.Media.Movie;

public class UpdateMovieDto
{
    public ISet<long> ActorIds { get; set; }
}
