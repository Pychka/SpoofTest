using SpoofTest.Models;

namespace SpoofTest.Services;

public interface IPersonRepository<T> : IRepository<T> where T : LoginEntity
{
    public Task<T?> GetByLoginAsync(string login);

    public Task<bool> AnyLoginAsync(string login);
    public Task<T?> GetWithIncludeByLoginAsync(string login, Func<IQueryable<T>, IQueryable<T>> include);
}
