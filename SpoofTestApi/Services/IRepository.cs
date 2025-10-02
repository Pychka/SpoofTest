using SpoofTestApi.Entities;

namespace SpoofTestApi.Services;

public interface IRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(int id);

    public Task EditAsync(T entity);

    public Task DeleteAsync(T entity);

    public Task AddAsync(T entity);
    public Task<List<T>> GetAll();
}
