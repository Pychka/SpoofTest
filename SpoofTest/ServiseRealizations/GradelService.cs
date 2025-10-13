using DataTransferObjects.Info;
using Microsoft.Extensions.Options;
using SpoofTest.Models;
using SpoofTest.Services;

namespace SpoofTest.ServiseRealizations
{
    public class GradelService(IOptions<GradingSystemConfig> config) : IGradelService
    {
        private readonly GradingSystemConfig config = config.Value;

        public TestResult GetGradel(int correctAnswers, int totalQuestions)
        {
            if (totalQuestions == 0)
                return new TestResult { Score = 2, RightCount = 0, AllCount = totalQuestions };

            var percentage = (double)correctAnswers / totalQuestions * 100;
            var roundedPercentage = Math.Round(percentage, config.Rounding);

            var rule = config.Rules.FirstOrDefault(r => roundedPercentage >= r.MinPercentage);

            return new TestResult
            {
                Score = rule?.Grade ?? 2,
                Percentage = roundedPercentage,
                RightCount = correctAnswers,
                AllCount = totalQuestions
            };
        }
    }
}
