using Microsoft.EntityFrameworkCore;
using SpoofTest.Models;
using SpoofTest.Services;
using System.Linq.Expressions;

namespace SpoofTest.ServiseRealizations;

public class BasePersonRepository<T>(SpoofTestContext context) : IPersonRepository<T> where T : LoginEntity
{
    private readonly SpoofTestContext context = context;
    private readonly DbSet<T> set = context.Set<T>();
    public async Task AddAsync(T entity)
    {
        await set.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        T? entity = await set.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Id is uncorrect");
        set.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<T?> GetByIdAsync(int key) =>
        await set.FirstOrDefaultAsync(x => x.Id == key);

    public async Task<bool> AnyLoginAsync(string login) =>
        await set.AnyAsync(x => x.Login == login);

    public async Task<T?> GetByLoginAsync(string login) =>
        await set.FirstOrDefaultAsync(x => x.Login == login);

    public async Task UpdateAsync() =>
        await context.SaveChangesAsync();

    public async Task<T?> GetWithIncludeByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include)
    {
        IQueryable<T> query = include(set);
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<T?> GetWithIncludeByLoginAsync(string login, Func<IQueryable<T>, IQueryable<T>> include)
    {
        IQueryable<T> query = include(set);
        return await query.FirstOrDefaultAsync(x => x.Login == login);
    }

    public async Task<List<T>> GetAllWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include)
    {
        IQueryable<T> query = include(set);
        return await query.Where(predicate).ToListAsync();
    }

    public async Task<T?> GetWithLinq(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include)
    {
        IQueryable<T> query = include(set);
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<List<T>> GetManyWithLinq(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include)
    {
        IQueryable<T> query = include(set);
        return await query.Where(predicate).ToListAsync();
    }
}