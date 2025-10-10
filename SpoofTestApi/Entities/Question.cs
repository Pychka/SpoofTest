namespace SpoofTestApi.Entities;

public partial class Question : BaseEntity
{
    public int TestId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = [];

    public virtual Test Test { get; set; } = null!;
}
public class QuestionDTOS : BaseEntity
{
    public int TestId { get; set; }

    public string Title { get; set; } = null!;

    public List<AnswerDTOS> Answers { get; set; } = [];
}
