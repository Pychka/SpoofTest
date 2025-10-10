using SpoofTestApi.Models;

namespace SpoofTestApi.Services;

public interface IPersonRepository<T> : IRepository<T> where T : LoginModel
{
    public Task<T?> GetByLoginAsync(string login);
    public Task<bool> AnyLoginAsync(string login);
}
