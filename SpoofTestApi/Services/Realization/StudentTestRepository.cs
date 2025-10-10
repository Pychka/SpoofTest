using Microsoft.EntityFrameworkCore;
using SpoofTestApi.Models;

namespace SpoofTestApi.Services.Realization;

public class StudentTestRepository(SpoofTestContext context)
{
    private readonly SpoofTestContext context = context;
    private readonly DbSet<StudentTest> set = context.Set<StudentTest>();
    public async Task AddAsync(StudentTest entity)
    {
        await set.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(StudentTest entity)
    {
        set.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task EditAsync(StudentTest entity)
    {
        set.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task<StudentTest?> GetByIdAsync(int studentId, int testId)
    {
        return await set.FirstOrDefaultAsync(x => x.StudentId == studentId && x.TestId == testId);
    }

    public async Task<List<Test>> GetAllAsync(int studentId)
    {
        return await set.Where(x => x.StudentId == studentId).Select(x => x.Test).ToListAsync();
    }
}
