namespace IMDB.Application.DTOs.Media.Movie;

public class UpdateTvShowDto
{
    public ISet<long> ActorIds { get; set; }
    public ISet<long> EpisodeIds { get; set; }
    public int? TotalSeasons { get; set; }
}