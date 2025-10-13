namespace DataTransferObjects.Info;

public class GroupInfo
{
    public string? Name { get; set; }

    public int Id { get; set; }

    public ICollection<StudentInfo> Students { get; set; } = [];
}
