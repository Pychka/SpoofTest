namespace SpoofTest.Models;

public partial class Test : Entity
{
    public int TeacherpId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int LimitMinutes { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<StudentTest> StudentTests { get; set; } = new List<StudentTest>();

    public virtual Teacher Teacherp { get; set; } = null!;
}
