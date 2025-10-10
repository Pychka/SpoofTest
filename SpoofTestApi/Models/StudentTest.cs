namespace SpoofTestApi.Models;

public partial class StudentTest
{
    public int StudentId { get; set; }

    public int TestId { get; set; }

    public DateTime? DeadLine { get; set; }

    public decimal Result { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = [];
}
