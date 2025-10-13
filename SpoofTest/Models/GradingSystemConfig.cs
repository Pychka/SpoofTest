namespace SpoofTest.Models;
public class GradingSystemConfig
{
    public List<GradingRule> Rules { get; set; } = new();
    public int Rounding { get; set; } = 2;
}