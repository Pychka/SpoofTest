using Microsoft.EntityFrameworkCore;
using SpoofTestApi.Models;

namespace SpoofTestApi.Services.Realization;

public class BasePersoneRepository<T>(SpoofTestContext context) : IPersonRepository<T> where T : LoginModel
{
    private readonly SpoofTestContext context = context;
    private readonly DbSet<T> set = context.Set<T>();
    public async Task AddAsync(T entity)
    {
        await set.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task<bool> AnyLoginAsync(string login)
    {
        return await set.AnyAsync(x => x.Login == login);
    }

    public async Task DeleteAsync(T entity)
    {
        set.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task EditAsync(T entity)
    {
        set.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await set.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T?> GetByLoginAsync(string Login)
    {
        return await set.FirstOrDefaultAsync(x => x.Login == Login);
    }
}
