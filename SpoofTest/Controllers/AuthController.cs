using DataTransferObjects.Edit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpoofTest.Models;
using SpoofTest.Services;

namespace SpoofTest.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(Hasher hasher, IPersonRepository<Student> studentRepository, IPersonRepository<Teacher> teacherRepository) : ControllerBase
{
    private readonly Hasher hasher = hasher;
    private readonly IPersonRepository<Student> studentRepository = studentRepository;
    private readonly IPersonRepository<Teacher> teacherRepository = teacherRepository;

    [HttpPost("Enter/Student")]
    public async Task<IActionResult> EnterStudent(string login, [FromBody] string password)
    {
        Student? student = await studentRepository.GetWithIncludeByLoginAsync(login, s => s.Include(s => s.Group));
        if (student is null)
            return NotFound("Login is uncorrect");
        if (!hasher.VerifyPassword(student.Password, password))
            return BadRequest("Password is uncorrect");
        return Ok(new StudentEdit()
        {
            Id = student.Id,
            LastName = student.LastName,
            Login = login,
            FirstName = student.FirstName,
            Group = new()
            {
                Id = student.Group.Id,
                Name = student.Group.Name,
            },
            Patronymic = student.Patronymic,
        });
    }

    [HttpPost("Enter/Teacher")]
    public async Task<IActionResult> EnterTeacher([FromBody] string password, string login)
    {
        Teacher? teacher = await teacherRepository.GetByLoginAsync(login);
        if (teacher is null)
            return NotFound("Login is uncorrect");
        if (!hasher.VerifyPassword(teacher.Password, password))
            return BadRequest("Password is uncorrect");
        return Ok(new TeacherEdit()
        {
            Id = teacher.Id,
            LastName = teacher.LastName,
            Login = login,
            FirstName = teacher.FirstName,
            Patronymic = teacher.Patronymic,
        });
    }

    [HttpPost("Create/Student")]
    public async Task<IActionResult> CreateStudent(StudentEdit student, string password)
    {
        if (await studentRepository.AnyLoginAsync(student.Login))
            return BadRequest("Login is busy");

        Student newStudent = new()
        {
            GroupId = student.Group?.Id ?? 0,
            LastName = student.LastName,
            Login = student.Login,
            FirstName = student.FirstName,
            Password = hasher.HashPassword(password),
            Patronymic = student.Patronymic,
        };
        await studentRepository.AddAsync(newStudent);
        return Ok("Success");
    }

    [HttpPost("Create/Teacher")]
    public async Task<IActionResult> CreateTeacher(TeacherEdit teacher, string password)
    {
        if (await teacherRepository.AnyLoginAsync(teacher.Login))
            return BadRequest("Login is busy");

        Teacher newTeacher = new()
        {
            LastName = teacher.LastName,
            Login = teacher.Login,
            FirstName = teacher.FirstName,
            Password = hasher.HashPassword(password),
            Patronymic = teacher.Patronymic,
        };
        await teacherRepository.AddAsync(newTeacher);
        return Ok("Success");
    }
}
