using DataTransferObjects.DTO;
using SpoofTestApi.Models;

namespace SpoofTestApi.Services;

public class Converter
{
    public TestDTOMini ConvertTestMini(Test test) =>
        new()
        {
            Id = test.Id,
            LimitMinutes = test.LimitMinutes,
            Teacher = ConvertTeacher(test.Teacher),
            Title = test.Title,
        };

    public TestDTO ConvertTest(Test test) =>
            new()
            {
                Id = test.Id,
                LimitMinutes = test.LimitMinutes,
                Title = test.Title,
                Questions = [.. test.Questions.Select(ConvertQuestion)],
            };

    public TeacherDTO ConvertTeacher(Teacher teacher) =>
        new()
        {
            Id = teacher.Id,
            LastName = teacher.LastName,
            Login = teacher.Login,
            Name = teacher.Name,
            Patronymic = teacher.Patronymic,
        };

    public QuestionDTO ConvertQuestion(Question question) =>
        new()
        {
            Answers = [.. question.Answers.Select(ConvertAnswer)],
            Id = question.Id,
            IsOnce = question.IsOnce,
            Title = question.Title,
        };

    public AnswerDTO ConvertAnswer(Answer answer) =>
        new()
        {
            Id = answer.Id,
            Title = answer.Title,
        };
}
