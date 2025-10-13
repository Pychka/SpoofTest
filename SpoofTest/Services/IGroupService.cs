using DataTransferObjects;

namespace SpoofTest.Services;

public interface IGroupService
{
    public Task<Result> Create(string name);
    public Task<Result> GetItSelf(int id, bool full);
    public Task<Result> GetStudents(int id);
    public Task<Result> AddStudentInGroup(int id, int studentId);
    public Task<Result> AddStudentsInGroup(int id, int[] studentsId);
}
