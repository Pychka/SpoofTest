using SpoofTest.Models;

namespace SpoofTest.Services
{
    public interface IGroupRepository : IRepository<Group>
    {
        public Task<bool> AnyAsync(string name);
    }
}
