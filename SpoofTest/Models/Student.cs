namespace SpoofTest.Models;

public partial class Student : LoginEntity
{
    public int GroupId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Password { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<StudentTest> StudentTests { get; set; } = new List<StudentTest>();
}
