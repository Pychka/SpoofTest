using DataTransferObjects.Server;
using Microsoft.AspNetCore.Mvc;
using SpoofTestApi.Entities;
using SpoofTestApi.Services;

namespace SpoofTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(IRepository<Test> test, IRepository<Submitted> submittedRepository, IRepository<Answer> answerRepository) : Controller
{
    private readonly IRepository<Test> testRepository = test;
    private readonly IRepository<Submitted> submittedRepository = submittedRepository;
    private readonly IRepository<Answer> answerRepository = answerRepository;
    [HttpGet("Score")]
    public async Task<IActionResult> Score(string password, int id, string? group)
    {
        if (password != "supersecurity") return BadRequest();
        var test = await testRepository.GetByIdAsync(id);
        if (test is null)
            return NotFound("Test is not found");
        ICollection<Submitted> submitted = group is null ? test.Submitteds : [.. test.Submitteds.Where(x => x.Group == group)];
        return Ok(test.Submitteds.Select(x => new Score()
        {
            Result = x.Result,
            Group = x.Group,
            LastName = x.LastName,
            Name = x.Name,
            Patronymic = x.Patronymic,
        }));
    }
    [HttpPost("CreateTest")]
    public async Task<IActionResult> ImportTestAsync(TestDTOS t, string password)
    {
        if (t == null || password != "supersecurity") return BadRequest();
        Test test = new()
        {
            Title = t.Title,
            Limit = t.Limit,
            Questions = [.. t.Questions.Select(x => new Question()
            {
                Title = x.Title,
                Answers = [.. x.Answers.Select(x => new Answer()
                {
                    IsCorrect = x.IsCorrect,
                    Title = x.Title,
                })]
            })],
        };
        await testRepository.AddAsync(test);
        return Ok();

    }

    
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var test = await testRepository.GetByIdAsync(id);
        return test is null ? NotFound() : Ok(Converter.ConvertTest(test));
    }
    [HttpPost]
    public async Task<IActionResult> SendAnswer([FromBody] DataTransferObjects.TestDTO answer)
    {
        Submitted submitted = new()
        {
            Group = answer.Group,
            LastName = answer.LastName,
            Name = answer.Name,
            Patronymic = answer.Patronymic,
            SessionId = answer.SessionId,
        };
        var test = await testRepository.GetByIdAsync(answer.TestId.Value);
        if (test is null)
            return NotFound("Нету");
        submitted.Test = test;
        foreach (var q in answer.Questions)
        {
            var correct = await answerRepository.GetByIdAsync(q.AnswerId);
            if (correct is null || correct.QuestionId != q.QuestionId)
                continue;
            submitted.Answers.Add(correct);
            if (correct.IsCorrect.Value is true)
                submitted.Result++;
        }
        await submittedRepository.AddAsync(submitted);
        return Ok(submitted.Result);
    }
}
