namespace SpoofTest.Models;

public partial class StudentTest : Entity
{
    public int StudentId { get; set; }

    public int TestId { get; set; }

    public int? Result { get; set; }

    public int AllPoints { get; set; }

    public int? Score { get; set; }

    public DateTime SetDate { get; set; }

    public DateTime? PassDate { get; set; }

    public DateTime? StartDate { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
