using DataTransferObjects.Edit;
using DataTransferObjects.Info;
using SpoofTest.Models;

namespace SpoofTest.Services;

public class Converter
{
    public Test ByEdit(TestEdit edit) =>
        new()
        {
            Description = edit.Description,
            Title = edit.Title ?? "",
            Questions = [.. edit.Questions?.Select(ByEdit) ?? []]
        };

    public Question ByEdit(QuestionEdit edit) =>
       new()
       {
           Content = edit.Title ?? "",
           Answers = [.. edit.Answers?.Select(ByEdit) ?? []],
           IsOnce = edit.IsOnce ?? true,
       };

    public Answer ByEdit(AnswerEdit edit) =>
       new()
       {
           Content = edit.Title ?? "",
           IsCorrect = edit.IsCorrect ?? false,
       };

    public TestInfo GetInfo(Test test, int? score, bool full) =>
        new()
        {
            Description = test.Description,
            Id = test.Id,
            LimitMinutes = test.LimitMinutes,
            Questions = full ? [.. test.Questions.Select(GetInfo)] : [],
            TeacherpId = test.TeacherpId,
            Title = test.Title,
            Score = score
        };

    public GroupInfo GetInfo(Group group, bool full) =>
        new()
        {
            Name = group.Name,
            Id = group.Id,
            Students = full ? [.. group.Students.Select(GetInfo)] : [],
        };

    public StudentInfo GetInfo(Student student) =>
        new()
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            Id = student.Id,
            Patronymic = student.Patronymic,
        };

    public QuestionInfo GetInfo(Question question) =>
        new()
        {
            Answers = [.. question.Answers.Select(GetInfo)],
            Content = question.Content,
            Id = question.Id,
            IsOnce = question.IsOnce,
        };

    public AnswerInfo GetInfo(Answer answer) =>
        new()
        {
            Content = answer.Content,
            Id = answer.Id,
        };


    public void SetValues(TestEdit edit, Test test)
    {
        test.Title = edit.Title ?? test.Title;
        test.LimitMinutes = edit.LimitMinutes ?? test.LimitMinutes;
        test.Description = edit.Description ?? test.Description;
    }

    public void SetValues(QuestionEdit edit, Question question)
    {
        question.Content = edit.Title ?? question.Content;
        question.IsOnce = edit.IsOnce ?? question.IsOnce;
    }

    public void SetValues(AnswerEdit edit, Answer answer)
    {
        answer.Content = edit.Title ?? answer.Content;
        answer.IsCorrect = edit.IsCorrect ?? answer.IsCorrect;
    }
}
