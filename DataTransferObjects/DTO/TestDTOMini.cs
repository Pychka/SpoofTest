namespace DataTransferObjects.DTO;

public class TestDTOMini
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int LimitMinutes { get; set; }

    public TeacherDTO Teacher { get; set; } = null!;
}
