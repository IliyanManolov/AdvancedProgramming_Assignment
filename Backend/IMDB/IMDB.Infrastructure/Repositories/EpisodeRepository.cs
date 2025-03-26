using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using System.IO;

namespace IMDB.Infrastructure.Repositories;

internal class EpisodeRepository : BaseRepository<ShowEpisode>, IEpisodeRepository
{
    public EpisodeRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }
}
