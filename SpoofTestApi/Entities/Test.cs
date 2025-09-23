namespace SpoofTestApi.Entities;

public partial class Test : BaseEntity
{
    public string Title { get; set; } = null!;

    public int? Limit { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = [];

    public virtual ICollection<Submitted> Submitteds { get; set; } = [];
}
public class TestDTOS : BaseEntity
{
    public string Title { get; set; } = null!;

    public int Limit { get; set; }

    public List<QuestionDTOS> Questions { get; set; } = [];

    public List<Submitted> Submitteds { get; set; } = [];
}
