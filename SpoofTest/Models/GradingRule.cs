namespace SpoofTest.Models;

public class GradingRule
{
    public int MinPercentage { get; set; }
    public int Grade { get; set; }
    public string Description { get; set; } = string.Empty;
}
