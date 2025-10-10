using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using SpoofTestApi.Models;
using SpoofTestApi.Secure;
using SpoofTestApi.Services;

namespace SpoofTestApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EntranceController(Hasher hasher, IPersonRepository<Student> studentRepository, IPersonRepository<Teacher> teacherRepository) : Controller
{
    private readonly Hasher hasher = hasher;
    private readonly IPersonRepository<Student> studentRepository = studentRepository;
    private readonly IPersonRepository<Teacher> teacherRepository = teacherRepository;

    [HttpPost("Enter/Student")]
    public async Task<IActionResult> EnterStudent(string login, [FromBody] string password)
    {
        Student? student = await studentRepository.GetByLoginAsync(login);
        if (student is null)
            return NotFound("Login is uncorrect");
        if (!hasher.VerifyPassword(student.Password, password))
            return BadRequest("Password is uncorrect");
        return Ok(new StudentDTO()
        {
            Id = student.Id,
            LastName = student.LastName,
            Login = login,
            Name = student.Name,
            Group = student.Group,
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
        return Ok(new TeacherDTO()
        {
            Id = teacher.Id,
            LastName = teacher.LastName,
            Login = login,
            Name = teacher.Name,
            Patronymic = teacher.Patronymic,
        });
    }

    [HttpPost("Create/Student")]
    public async Task<IActionResult> CreateStudent(StudentDTO student, string password)
    {
        if (await studentRepository.AnyLoginAsync(student.Login))
            return BadRequest("Login is busy");

        Student newStudent = new()
        {
            Group = student.Group,
            LastName = student.LastName,
            Login = student.Login,
            Name = student.Name,
            Password = hasher.HashPassword(password),
            Patronymic = student.Patronymic,
        };
        await studentRepository.AddAsync(newStudent);
        return Ok("Success");
    }

    [HttpPost("Create/Teacher")]
    public async Task<IActionResult> CreateTeacher(TeacherDTO teacher, string password)
    {
        if (await teacherRepository.AnyLoginAsync(teacher.Login))
            return BadRequest("Login is busy");

        Teacher newTeacher = new()
        {
            LastName = teacher.LastName,
            Login = teacher.Login,
            Name = teacher.Name,
            Password = hasher.HashPassword(password),
            Patronymic = teacher.Patronymic,
        };
        await teacherRepository.AddAsync(newTeacher);
        return Ok("Success");
    }
}
