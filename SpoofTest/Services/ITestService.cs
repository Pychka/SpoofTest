using DataTransferObjects;
using DataTransferObjects.Edit;
using DataTransferObjects.Reply;

namespace SpoofTest.Services;

public interface ITestService
{
    public Task<Result> EditTestAsync(TestEdit test, int teacherId);

    public Task<Result> GetTestInfoAsync(int testId, bool full);

    public Task<Result> GetTestsInfoAsync(int pesoneId, PersoneType type);

    public Task<Result> ReplyTestAsync(TestReply reply, int studentId);
    public Task<Result> AsignAsync(int testId, int studentId, DateTime setDate, DateTime? passDate);
    public Task<Result> AsignGroupAsync(int testId, int groupId, DateTime setDate, DateTime? passDate);
}
