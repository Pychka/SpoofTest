using Microsoft.EntityFrameworkCore;
using SpoofTestApi.Entities;

namespace SpoofTestApi.Services.Realization;

public class BaseRepository<T>(SpoofTestContext context) : IRepository<T> where T : BaseEntity
{
    private readonly SpoofTestContext context = context;
    private readonly DbSet<T> set = context.Set<T>();
    public async Task AddAsync(T entity)
    {
        await set.AddAsync(entity);
        await context.SaveChangesAsync();
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

    public async Task<List<T>> GetAll()
    {
        return await set.ToListAsync();
    }
}
