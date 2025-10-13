using DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using SpoofTest.Models;
using SpoofTest.Services;

namespace SpoofTest.ServiseRealizations;

public class GroupService(IGroupRepository groupRepository, IPersonRepository<Student> studentRepository, Converter converter) : IGroupService
{
    private readonly IGroupRepository groupRepository = groupRepository;
    private readonly IPersonRepository<Student> studentRepository = studentRepository;
    private readonly Converter converter = converter;

    public async Task<Result> AddStudentInGroup(int id, int studentId)
    {
        Group? group = await groupRepository.GetByIdAsync(id);
        if (group is null)
            return Result.NotFoundResult("Incorrect groupId");
        Student? student = await studentRepository.GetWithIncludeByIdAsync(studentId, s => s.Include(s => s.Group));
        if (student is null)
            return Result.NotFoundResult("Incrorrect studentId");
        if (student.Group is not null)
            return Result.NotFoundResult("Student has group");
        student.GroupId = group.Id;
        await studentRepository.UpdateAsync();
        return Result.SuccessResult("Ok");
    }

    public async Task<Result> AddStudentsInGroup(int id, int[] studentsId)
    {
        Group? group = await groupRepository.GetWithIncludeByIdAsync(id, g => g.Include(g => g.Students));
        if (group is null)
            return Result.NotFoundResult("Incorrect groupId");
        
        foreach(var studentId in studentsId)
        {
            Student? student = await studentRepository.GetWithIncludeByIdAsync(studentId, s => s.Include(s => s.Group));
            if (student is null || student.Group is not null)
                continue;
            group.Students.Add(student);
        }

        await groupRepository.UpdateAsync();
        return Result.SuccessResult("Ok");
    }

    public async Task<Result> Create(string name)
    {
        if (await groupRepository.AnyAsync(name))
            return Result.ErrorResult("Name is busy");
        Group group = new()
        {
            Name = name,
        };
        await groupRepository.AddAsync(group);
        return Result.SuccessResult("Ok", group.Id);
    }

    public async Task<Result> GetItSelf(int id, bool full)
    {
        Group? group = await groupRepository.GetByIdAsync(id);
        if (group is null)
            return Result.NotFoundResult("Incorrect groupId");
        return Result.SuccessResult("Ok", converter.GetInfo(group, full));
    }

    public async Task<Result> GetStudents(int id)
    {
        Group? group = await groupRepository.GetByIdAsync(id);
        if (group is null)
            return Result.NotFoundResult("Incorrect groupId");

        return Result.SuccessResult("Ok", group.Students.Select(converter.GetInfo));
    }
}
