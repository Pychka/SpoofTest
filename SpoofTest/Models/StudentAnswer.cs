namespace SpoofTest.Models;

public partial class StudentAnswer : Entity
{
    public int StudentTestId { get; set; }

    public int AnswerId { get; set; }

    public virtual Answer Answer { get; set; } = null!;

    public virtual StudentTest StudentTest { get; set; } = null!;
}
