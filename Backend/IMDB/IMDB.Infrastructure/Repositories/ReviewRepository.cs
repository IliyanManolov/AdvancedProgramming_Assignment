using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.Models;
using IMDB.Infrastructure.Database;

namespace IMDB.Infrastructure.Repositories;

internal class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(DatabaseContext dbContext) : base(dbContext)
    {

    }
}