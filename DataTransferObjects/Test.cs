namespace DataTransferObjects;

public class TestDTO
{
    public int? TestId { get; set; } = null!;
    public List<QuestionDTO> Questions { get; set; } = [];
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Group { get; set; } = null!;
    public string SessionId { get; set; } = null!;
}
