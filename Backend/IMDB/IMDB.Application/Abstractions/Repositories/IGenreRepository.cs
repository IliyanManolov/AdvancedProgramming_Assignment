using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface IGenreRepository : IBaseRepository<Genre>
{
    public Task<Genre?> GetByGenreNameAsync(string? genreName);
    public Task<IEnumerable<Genre>> GetByGenreNameBulkAsync(IEnumerable<string>? genreNames);
}
