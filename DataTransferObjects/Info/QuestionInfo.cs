namespace DataTransferObjects.Info;

public class QuestionInfo
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public bool IsOnce { get; set; }

    public virtual ICollection<AnswerInfo> Answers { get; set; } = [];
}
