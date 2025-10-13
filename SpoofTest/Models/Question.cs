namespace SpoofTest.Models;

public partial class Question : Entity
{
    public int TestId { get; set; }

    public string Content { get; set; } = null!;

    public bool IsOnce { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Test Test { get; set; } = null!;
}
