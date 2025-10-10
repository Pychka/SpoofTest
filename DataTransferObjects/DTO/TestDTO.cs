namespace DataTransferObjects.DTO;

public class TestDTO
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int LimitMinutes { get; set; }

    public List<QuestionDTO> Questions { get; set; } = [];
}
