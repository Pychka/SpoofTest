using DataTransferObjects.Edit;
using DataTransferObjects.Reply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpoofTest.Models;
using SpoofTest.Services;

namespace SpoofTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(ITestService testService) : ControllerBase
{
    private readonly ITestService testService = testService;

    [HttpGet("Info")]
    public async Task<IActionResult> GetInfo(int testId, bool full)
    {
        if (testId < 0)
            return BadRequest("Invalid testId");
        var result = await testService.GetTestInfoAsync(testId, full);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Info/Many")]
    public async Task<IActionResult> GetInfo(int personeId, PersoneType persone)
    {
        if (personeId < 0)
            return BadRequest("Invalid testId");
        var result = await testService.GetTestsInfoAsync(personeId, persone);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("Edit")]
    public async Task<IActionResult> Edit(TestEdit test, int teacherId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (teacherId < 0)
            return Forbid("Invalid teacherId");

        var result = await testService.EditTestAsync(test, teacherId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("Reply")]
    public async Task<IActionResult> Reply(TestReply reply, int studentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (studentId < 0)
            return Forbid("Invalid studentId");

        var result = await testService.ReplyTestAsync(reply, studentId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Asign")]
    public async Task<IActionResult> Asign(int testId, int studentId, DateTime setDate, DateTime? passDate)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (testId < 0 || studentId < 0)
            return Forbid("Invalid studentId or testId");

        var result = await testService.AsignAsync(testId, studentId, setDate, passDate);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("AsignByGroup")]
    public async Task<IActionResult> AsignByGroup(int testId, int groupId, DateTime setDate, DateTime? passDate)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (testId < 0 || groupId < 0)
            return Forbid("Invalid groupId or testId");

        var result = await testService.AsignGroupAsync(testId, groupId, setDate, passDate);
        return StatusCode(result.StatusCode, result);
    }

    /*private static (int?, string message) GetId(HttpContext context, PersoneType needable, string errorMessage)
    {
        if (!int.TryParse(context.User.FindFirst("UserId")?.Value, out int userId))
            return (null, "InvalidId");

        if (!Enum.TryParse(typeof(PersoneType), context.User.FindFirst("PersoneType")?.Value, out object? type) || type is not PersoneType persone)
            return (null, "Invalid user role");

        if (persone != needable)
            return (null, errorMessage);

        return (userId, "");
    }*/
}