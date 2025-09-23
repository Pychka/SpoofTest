namespace SpoofTestApi.Entities;

public partial class Answer : BaseEntity
{
    public int QuestionId { get; set; }

    public string Title { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<Submitted> Submitteds { get; set; } = new List<Submitted>();
}
public class AnswerDTOS : BaseEntity
{
    public int QuestionId { get; set; }

    public string Title { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public List<Submitted> Submitteds { get; set; } = new List<Submitted>();
}
