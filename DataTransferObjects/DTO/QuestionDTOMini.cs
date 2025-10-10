namespace DataTransferObjects.DTO;

public class QuestionDTOMini
{
    public int Id { get; set; }

    public List<int> AnswersId { get; set; } = [];
}
