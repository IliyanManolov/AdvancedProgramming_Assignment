using IMDB.Application.Abstractions.Repositories;
using IMDB.Domain.AbstractModels;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Repositories;

internal abstract class BaseRepository<T> : IBaseRepository<T> where T : DomainEntity
{

    private readonly DatabaseContext _dbContext;
    protected IQueryable<T> Query => _dbContext.Set<T>();
    protected BaseRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(long? id)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Set<T>()
            .Entry(entity).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
        return entity;
    }
}
