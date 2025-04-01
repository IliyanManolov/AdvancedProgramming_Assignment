using IMDB.Application.Converters;
using System.Text.Json.Serialization;

namespace IMDB.Application.DTOs.ShowEpisode;

public class EpisodeDetailsDto
{
    [JsonConverter(typeof(JsonDateConverter))]
    public DateTime DateAired { get; set; }
    public long? Length { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? SeasonNumber { get; set; }
    public double? Rating { get; set; }
    public long? Reviews { get; set; }
}
