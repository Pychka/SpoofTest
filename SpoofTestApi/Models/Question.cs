namespace SpoofTestApi.Models;

public partial class Question : BaseEntity
{
    public int TestId { get; set; }

    public bool? IsOnce { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = [];

    public virtual Test Test { get; set; } = null!;
}
