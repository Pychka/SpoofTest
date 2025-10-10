namespace DataTransferObjects.DTO;

public class TestDTOFull
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int LimitMinutes { get; set; }

    public List<QuestionDTOFull> Questions { get; set; } = [];
}
