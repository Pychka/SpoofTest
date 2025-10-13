using SpoofTest.Models;
using System.Linq.Expressions;

namespace SpoofTest.Services;

public interface IRepository<T> where T : Entity
{
    public Task AddAsync(T entity);

    public Task UpdateAsync();

    public Task DeleteAsync(int entity);

    public Task<T?> GetByIdAsync(int key);

    public Task<T?> GetWithLinq(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include);

    public Task<List<T>> GetManyWithLinq(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include);

    public Task<T?> GetWithIncludeByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include);

    public Task<List<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include);
}