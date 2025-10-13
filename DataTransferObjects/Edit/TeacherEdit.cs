namespace DataTransferObjects.Edit;

public class TeacherEdit
{
    public int Id { get;set; } 

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string? Patronymic { get; set; }
}
