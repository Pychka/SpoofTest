namespace DataTransferObjects.Edit;

public class AnswerEdit
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool? IsCorrect { get; set; }

    public State State { get; set; }
}
