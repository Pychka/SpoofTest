using Microsoft.EntityFrameworkCore;
using SpoofTest.Models;
using SpoofTest.Services;
using System.Linq.Expressions;

namespace SpoofTest.ServiseRealizations
{
    public class GroupRepository(SpoofTestContext context) : IGroupRepository
    {
        private readonly SpoofTestContext context = context;
        private readonly DbSet<Group> set = context.Set<Group>();
        public async Task AddAsync(Group entity)
        {
            await set.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Group? entity = await set.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Id is uncorrect");
            set.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Group?> GetByIdAsync(int key) =>
            await set.FirstOrDefaultAsync(x => x.Id == key);

        public async Task UpdateAsync() =>
            await context.SaveChangesAsync();

        public async Task<Group?> GetWithIncludeByIdAsync(int id, Func<IQueryable<Group>, IQueryable<Group>> include)
        {
            IQueryable<Group> query = include(set);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Group>> GetAllWithIncludeAsync(Expression<Func<Group, bool>> predicate, Func<IQueryable<Group>, IQueryable<Group>> include)
        {
            IQueryable<Group> query = include(set);
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<bool> AnyAsync(string name) =>
            await set.AnyAsync(x => x.Name == name);

        public async Task<Group?> GetWithLinq(Expression<Func<Group, bool>> predicate, Func<IQueryable<Group>, IQueryable<Group>> include)
        {
            IQueryable<Group> query = include(set);
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<Group>> GetManyWithLinq(Expression<Func<Group, bool>> predicate, Func<IQueryable<Group>, IQueryable<Group>> include)
        {
            IQueryable<Group> query = include(set);
            return await query.Where(predicate).ToListAsync();
        }
    }
}
