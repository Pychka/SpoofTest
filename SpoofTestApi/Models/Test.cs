namespace SpoofTestApi.Models;

public partial class Test : BaseEntity
{
    public string Title { get; set; } = null!;

    public int LimitMinutes { get; set; }

    public int TeacherId { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = [];

    public virtual ICollection<StudentTest> StudentTests { get; set; } = [];

    public virtual Teacher Teacher { get; set; } = null!;
}
