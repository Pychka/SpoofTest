namespace SpoofTestApi.Entities;

public partial class Submitted : BaseEntity
{
    public decimal Result { get; set; }

    public int TestId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Group { get; set; } = null!;

    public string SessionId { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
