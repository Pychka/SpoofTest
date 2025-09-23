namespace DataTransferObjects.Server;

public class TestDTOAnswer
{
    public int Id { get;set; }

    public List<QuestionDTO> Questions { get; set; } = [];

    public string Title { get; set; } = null!;

    public int LimitMinutes { get; set; }
}
