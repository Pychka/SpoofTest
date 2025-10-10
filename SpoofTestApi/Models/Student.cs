namespace SpoofTestApi.Models;

public partial class Student : LoginModel
{
    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Group { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = [];

    public virtual ICollection<StudentTest> StudentTests { get; set; } = [];
}
