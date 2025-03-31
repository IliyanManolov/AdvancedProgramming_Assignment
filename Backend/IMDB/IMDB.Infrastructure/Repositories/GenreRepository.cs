using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal class GenreRepository : BaseRepository<Genre>, IGenreRepository
{
    public GenreRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<Genre?> GetByGenreNameAsync(string? genreName)
    {
        return await Query
            .Where(e => e.Name.Equals(genreName))
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Genre>> GetByGenreNameBulkAsync(IEnumerable<string>? genreNames)
    {
        var genreNamesSet = new HashSet<string>(genreNames);
        return await Query
            .Where(e => genreNamesSet.Contains(e.Name))
            .ToListAsync();
    }
}