namespace DataTransferObjects.Server;

public class QuestionDTO
{
    public int Id { get; set; }

    public List<AnswerDTO> Answers { get; set; } = [];

    public string Title { get; set; } = null!;
}
