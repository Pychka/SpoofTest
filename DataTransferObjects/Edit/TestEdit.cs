namespace DataTransferObjects.Edit;

public class TestEdit
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? LimitMinutes { get; set; }

    public State State { get; set; }

    public ICollection<QuestionEdit>? Questions { get; set; }
}
