namespace DataTransferObjects.DTO;

public class AnswerDTOFull
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsCorrect { get; set; }
}