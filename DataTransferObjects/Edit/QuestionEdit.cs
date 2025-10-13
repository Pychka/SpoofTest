namespace DataTransferObjects.Edit;

public class QuestionEdit
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool? IsOnce { get; set; }

    public State State { get; set; }

    public ICollection<AnswerEdit>? Answers { get; set; }
}
