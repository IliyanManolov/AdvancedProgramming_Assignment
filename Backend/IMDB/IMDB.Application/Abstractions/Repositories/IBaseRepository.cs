using IMDB.Domain.AbstractModels;

namespace IMDB.Application.Abstractions.Repositories;

public interface IBaseRepository<T>
    where T : DomainEntity
{
    public Task<T?> GetByIdAsync(long? id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> CreateAsync(T entity);
    public Task<T> DeleteAsync(T entity);
    public Task<T> UpdateAsync(T entity);
}
