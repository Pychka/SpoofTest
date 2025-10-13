using DataTransferObjects;
using DataTransferObjects.Edit;
using DataTransferObjects.Info;
using DataTransferObjects.Reply;
using Microsoft.EntityFrameworkCore;
using SpoofTest.Models;
using SpoofTest.Services;

namespace SpoofTest.ServiseRealizations;

public class TestService(IRepository<Test> testRepository, IRepository<StudentTest> studentTestRepository, IRepository<Question> questionRepository, SpoofTestContext context, IPersonRepository<Student> studentRepository, IGroupRepository groupRepository, Converter converter, IGradelService gradelService) : ITestService
{
    private readonly IRepository<Test> testRepository = testRepository;
    private readonly IRepository<StudentTest> studentTestRepository = studentTestRepository;
    private readonly IPersonRepository<Student> studentRepository = studentRepository;
    private readonly IGroupRepository groupRepository = groupRepository;
    private readonly IRepository<Question> questionRepository = questionRepository;
    private readonly SpoofTestContext context = context;
    private readonly Converter converter = converter;
    private readonly IGradelService gradelService = gradelService;

    public async Task<Result> AsignAsync(int testId, int studentId, DateTime setDate, DateTime? passDate)
    {
        Test? test = await testRepository.GetWithIncludeByIdAsync(testId, x => x.Include(t => t.StudentTests));
        if (test is null)
            return Result.NotFoundResult("Not found");

        Student? student = await studentRepository.GetByIdAsync(studentId);
        if (student is null)
            return Result.NotFoundResult("Not found");

        StudentTest studentTest = new()
        {
            TestId = test.Id,
            StudentId = student.Id,
            SetDate = setDate,
            PassDate = passDate
        };

        await studentTestRepository.AddAsync(studentTest);
        return Result.SuccessResult("Ok");
    }

    public async Task<Result> AsignGroupAsync(int testId, int groupId, DateTime setDate, DateTime? passDate)
    {
        Test? test = await testRepository.GetWithIncludeByIdAsync(testId, x => x.Include(t => t.StudentTests).Include(q => q.Questions));
        if (test is null)
            return Result.NotFoundResult("Not found");

        Group? group = await groupRepository.GetWithIncludeByIdAsync(groupId, g => g.Include(g => g.Students));
        if (group is null)
            return Result.NotFoundResult("Not found");

        foreach(var student in group.Students)
        {
            StudentTest studentTest = new()
            {
                TestId = test.Id,
                StudentId = student.Id,
                SetDate = setDate,
                PassDate = passDate,
                AllPoints = test.Questions.Count,

            };
            test.StudentTests.Add(studentTest);
        }

        await testRepository.UpdateAsync();
        return Result.SuccessResult("Ok");
    }

    public async Task<Result> EditTestAsync(TestEdit test, int teacherId)
    {
        try
        {
            if (test.State is State.Delete)
            {
                Test? testToDelete = await testRepository.GetByIdAsync(test.Id);
                if (testToDelete is null)
                    return Result.NotFoundResult("Test is not found");
                if (testToDelete.TeacherpId != teacherId)
                    return Result.ErrorResult("Broken teacherId", 403);

                await testRepository.DeleteAsync(test.Id);
                return Result.DeletedResult("Test was deleted");
            }
            else if (test.State is State.Add)
            {
                Test added = converter.ByEdit(test);
                added.TeacherpId = teacherId;
                await testRepository.AddAsync(added);
                return Result.SuccessResult("Test was added");
            }
            else
            {
                Test? changed = await testRepository.GetWithIncludeByIdAsync(test.Id, x => x.Include(t => t.Questions).ThenInclude(q => q.Answers));
                if (changed is null || changed.TeacherpId != teacherId)
                    return Result.ErrorResult("Test is not found or broken teacherId", 404);

                ChangeQuestions(changed, test.Questions);
                converter.SetValues(test, changed);

                await testRepository.UpdateAsync();
                return Result.SuccessResult("Test was edited");
            }
        }
        catch (Exception ex)
        {
            return Result.ErrorResult(ex.Message);
        }
    }

    public async Task<Result> GetTestInfoAsync(int testId, bool full)
    {
        Test? test = await testRepository.GetWithIncludeByIdAsync(testId, x => x.Include(t => t.Questions).ThenInclude(a => a.Answers));
        return test is null
            ? Result.NotFoundResult("Test is not exists")
            : Result.SuccessResult("Ok", converter.GetInfo(test, null, full));
    }
    public async Task<Result> GetTestsInfoAsync(int personeId, PersoneType type)
    {
        if (type is PersoneType.Teacher)
        {
            List<Test> test = await testRepository.GetManyWithLinq(x => x.TeacherpId == personeId, x => x.Include(x => x.Questions).ThenInclude(x => x.Answers));
            return Result.SuccessResult("Ok", test.Select(x => converter.GetInfo(x,null, true)));
        }
        else
        {
            List<StudentTest> test = await studentTestRepository.GetManyWithLinq(x => x.StudentId == personeId, x => x.Include(x => x.Test).ThenInclude(x => x.Questions).ThenInclude(x => x.Answers));
            return Result.SuccessResult("Ok", test.Select(x => converter.GetInfo(x.Test, x.Score, true)));
        }
    }

    public async Task<Result> ReplyTestAsync(TestReply reply, int studentId)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            StudentTest? studentTest = await studentTestRepository.GetWithIncludeByIdAsync(reply.Id, x => x.Include(st => st.StudentAnswers));

            if (studentTest is null)
                return Result.NotFoundResult("Broken id");

            if (studentTest.StudentId != studentId)
                return Result.ErrorResult("Not your test", 403);

            if (studentTest.Result.HasValue)
                return Result.ErrorResult("Test already completed", 400);

            var testQuestions = await questionRepository.GetAllWithIncludeAsync(q => q.TestId == studentTest.TestId, q => q.Include(q => q.Answers));
            var questionsDict = testQuestions.ToDictionary(q => q.Id);

            if (reply.Questions is null)
            {
                studentTest.Result = 0;
                await studentTestRepository.UpdateAsync();
                await transaction.CommitAsync();
                return Result.SuccessResult("Your result: 0");
            }

            int result = 0;

            foreach (var questionReply in reply.Questions)
                if (questionsDict.TryGetValue(questionReply.Id, out var question))
                    result += GetResult(questionReply, question, studentTest);
            TestResult testResult = gradelService.GetGradel(result, studentTest.AllPoints);
            studentTest.Result = result;
            studentTest.Score = testResult.Score;
            studentTest.PassDate = DateTime.Now;
            await studentTestRepository.UpdateAsync();
            await transaction.CommitAsync();

            return Result.SuccessResult($"Your score: {testResult.Score}", testResult);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.ErrorResult(ex.Message);
        }
    }

    private static int GetResult(QuestionReply reply, Question question, StudentTest studentTest)
    {
        ICollection<int> correct = [];
        var correctAnswers = question.Answers.Where(x => x.IsCorrect).Select(x => x.Id);
        if (reply.Answers is null || question is null || question.TestId != studentTest.TestId)
            return 0;

        var distinctAnswers = reply.Answers?.Distinct().ToArray() ?? [];

        foreach (var answerId in distinctAnswers)
        {
            Answer? answer = question.Answers.FirstOrDefault(x => x.Id == answerId);
            if (answer is null || answer.QuestionId != question.Id) continue;

            StudentAnswer studentAnswer = new()
            {
                AnswerId = answer.Id,
                StudentTestId = studentTest.Id,
            };
            studentTest.StudentAnswers.Add(studentAnswer);

            correct.Add(answer.Id);
        }

        return correct.Order().SequenceEqual(correctAnswers.Order()) ? 1 : 0;
    }

    private void ChangeQuestions(Test test, ICollection<QuestionEdit>? questionEdits)
    {
        if (questionEdits is null)
            return;

        var dict = test.Questions.ToDictionary(q => q.Id);
        List<Question> toRemove = [];

        foreach (var dto in questionEdits)
        {
            if (dto.State is State.Delete)
            {
                if (dict.TryGetValue(dto.Id, out var question))
                    toRemove.Add(question);
                continue;
            }

            if (dict.TryGetValue(dto.Id, out var existingQuestion))
            {
                converter.SetValues(dto, existingQuestion);
                ChangeAnswers(existingQuestion, dto.Answers);
            }

            else
            {
                var question = converter.ByEdit(dto);
                question.TestId = test.Id;
                test.Questions.Add(question);
            }
        }

        foreach (var question in toRemove)
            test.Questions.Remove(question);
    }
    private void ChangeAnswers(Question question, ICollection<AnswerEdit>? answerEdits)
    {
        if (answerEdits is null)
            return;

        var dict = question.Answers.ToDictionary(q => q.Id);
        List<Answer> toRemove = [];

        foreach (var dto in answerEdits)
        {
            if (dto.State is State.Delete)
            {
                if (dict.TryGetValue(dto.Id, out var answer))
                    toRemove.Add(answer);
                continue;
            }

            if (dict.TryGetValue(dto.Id, out var existingAnswer))
                converter.SetValues(dto, existingAnswer);

            else
            {
                var answer = converter.ByEdit(dto);
                answer.QuestionId = question.Id;
                question.Answers.Add(answer);
            }
        }

        foreach (var answer in toRemove)
            question.Answers.Remove(answer);
    }
}
