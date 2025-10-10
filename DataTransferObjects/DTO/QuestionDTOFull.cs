namespace DataTransferObjects.DTO;

public class QuestionDTOFull
{
    public int Id { get; set; }

    public bool? IsOnce { get; set; }

    public string Title { get; set; } = null!;

    public List<AnswerDTO> Answers { get; set; } = [];
}
