using IMDB.Application.DTOs.Media;
using IMDB.Domain.Models;

namespace IMDB.Application.Abstractions.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
    public Task<IEnumerable<Review>> GetAllForMovieAsync(long id);
    public Task<IEnumerable<Review>> GetAllForShowAsync(long id);
    public Task<IEnumerable<Review>> GetAllForEpisodeAsync(long id);
}