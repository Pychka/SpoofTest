namespace SpoofTestApi.Models;

public partial class Teacher : LoginModel
{
    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = [];
}
