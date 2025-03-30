using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IMDB.Application.DTOs.Media;

/// <summary>
/// DTO returning basic information for a media type. Intended for discovery
/// </summary>
public class MediaShortDto
{
    public long? Id { get; set; }
    /// <summary>
    /// Names of genres
    /// </summary>
    public ISet<string> Genres { get; set; } = new HashSet<string>();
    /// <summary>
    /// Full names Names of directors
    /// </summary>
    public string Director { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Title { get; set; }
    public double? Rating { get; set; }
    public long Reviews { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// Property to allow frontend to distinguish between Movie, TvShow, etc.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MediaType Type { get; set; }

    public long Length { get; set; }

    public int? ShowSeasonsCount { get; set; }
    public long? ShowEpisodesCount { get; set; }
    public DateTime? ShowEndDate { get; set; }
}
