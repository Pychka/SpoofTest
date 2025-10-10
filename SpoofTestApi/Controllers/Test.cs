using DataTransferObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using SpoofTestApi.Models;
using SpoofTestApi.Services;
using SpoofTestApi.Services.Realization;

namespace SpoofTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(Converter converter, IRepository<Question> questionRepository, IRepository<Test> test, StudentTestRepository studentTestRepository, IRepository<Answer> answerRepository) : Controller
{
    private readonly Converter converter = converter;
    private readonly IRepository<Test> testRepository = test;
    private readonly StudentTestRepository studentTestRepository = studentTestRepository;
    private readonly IRepository<Answer> answerRepository = answerRepository;
    private readonly IRepository<Question> questionRepository = questionRepository;

    [HttpGet("Test/One")]
    public async Task<IActionResult> Get(int id)
    {
        var test = await testRepository.GetByIdAsync(id);
        return test is null ? NotFound() : Ok(converter.ConvertTest(test));
    }

    [HttpGet("Test/Many")]
    public async Task<IActionResult> GetMany(int studentId)
    {
        List<Test> test = await studentTestRepository.GetAllAsync(studentId);
        return test is null ? NotFound() : Ok(test.Select(converter.ConvertTestMini));
    }

    [HttpPost]
    public async Task<IActionResult> SendAnswer(ResponseTestDTO answer)
    {
        StudentTest? studentTest = await studentTestRepository.GetByIdAsync(answer.StudentId, answer.Id);
        if (studentTest is null)
            return NotFound("Test or you is not exist");

        var tasks = answer.Questions.Select(async x =>
        {
            Question? question = await questionRepository.GetByIdAsync(x.Id);
            if (question == null)
                return [];

            if (question.IsOnce is true)
            {
                if (x.AnswersId.Count == 0)
                    return [];

                var answerId = x.AnswersId[0];
                var answerEntity = await answerRepository.GetByIdAsync(answerId);
                return answerEntity != null ? [answerEntity] : [];
            }
            else
            {
                var answerTasks = x.AnswersId.Select(id => answerRepository.GetByIdAsync(id));
                var answers = await Task.WhenAll(answerTasks);
                return answers.Where(a => a != null).Cast<Answer>();
            }
        });

        var results = await Task.WhenAll(tasks);

        studentTest.Answers = [.. results.SelectMany(x => x)];

        studentTest.Result = studentTest.Answers.Where(x => x.IsCorrect is true).Count();

        await studentTestRepository.AddAsync(studentTest);
        return Ok(studentTest.Result);
    }

    [HttpPost("Test/Create")]
    public async Task<IActionResult> CreateTest(TestDTOFull test)
    {
        return BadRequest("In development");
    }
}
