namespace DataTransferObjects.DTO;

public class StudentDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Group { get; set; } = null!;

    public string Login { get; set; } = null!;
}
