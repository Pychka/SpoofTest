namespace DataTransferObjects.DTO;

public class TeacherDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Login { get;set; } = null!;
}
