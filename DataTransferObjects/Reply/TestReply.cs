namespace DataTransferObjects.Reply;

public class TestReply
{
    public int Id { get;set; }
    public ICollection<QuestionReply>? Questions { get; set; }
}
