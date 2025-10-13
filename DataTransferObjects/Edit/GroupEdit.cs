namespace DataTransferObjects.Edit;

public class GroupEdit
{
    public string? Name { get; set; }

    public int Id { get; set; }

    public State State { get; set; }

    public int[]? StudentIds { get; set; }
}
