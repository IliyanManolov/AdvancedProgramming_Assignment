using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Application.DTOs.ShowEpisode;

public class CreateEpisodeDto
{
    [Required]
    public DateTime DateAired { get; set; }
    [Required]
    public long Length { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public int? SeasonNumber { get; set; }

    [Required]
    public long ShowId { get; set; }
    [Required]
    public long CreatedByUserId { get; set; }
}
