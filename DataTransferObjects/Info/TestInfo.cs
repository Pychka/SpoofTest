namespace DataTransferObjects.Info;

public class TestInfo
{
    public int Id { get; set; }

    public int TeacherpId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int LimitMinutes { get; set; }

    public int? Score { get; set; }

    public virtual ICollection<QuestionInfo> Questions { get; set; } = [];
}
