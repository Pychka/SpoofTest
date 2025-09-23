using DataTransferObjects.Server;

namespace SpoofTestApi.Services;

public static class Converter
{
    public static TestDTOAnswer ConvertTest(Entities.Test test)
    {
        return new()
        {
            Id = test.Id,
            Title = test.Title,
            LimitMinutes = test.Limit ?? 0,
            Questions = [.. test.Questions.Select(
                x => new QuestionDTO()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Answers = [.. x.Answers.Select(x => new AnswerDTO()
                    {
                        Id = x.Id,
                        Title = x.Title
                    })]
                })]
        };
    }
}
