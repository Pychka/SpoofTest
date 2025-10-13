namespace SpoofTest.Models;

public partial class Group : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
