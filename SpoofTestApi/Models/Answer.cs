namespace SpoofTestApi.Models;

public partial class Answer : BaseEntity
{
    public int QuestionId { get; set; }

    public string Title { get; set; } = null!;

    public bool? IsCorrect { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<StudentTest> StudentTests { get; set; } = [];
}
