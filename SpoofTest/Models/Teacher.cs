namespace SpoofTest.Models;

public partial class Teacher : LoginEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
