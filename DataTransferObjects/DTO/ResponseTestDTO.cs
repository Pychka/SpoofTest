namespace DataTransferObjects.DTO;

public class ResponseTestDTO
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public List<QuestionDTOMini> Questions { get; set; } = [];
}
