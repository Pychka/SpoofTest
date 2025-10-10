namespace DataTransferObjects.Server;

public class Score
{
    public decimal Result { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Group { get; set; } = null!;
}
