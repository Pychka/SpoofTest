using DataTransferObjects.Info;

namespace SpoofTest.Services;

public interface IGradelService
{
    public TestResult GetGradel(int correctAnswers, int totalQuestions);
}
