using Microsoft.AspNetCore.Mvc;
using SpoofTest.Services;

namespace SpoofTest.Controllers;

public class GroupController(IGroupService groupService) : ControllerBase
{
    private readonly IGroupService groupService = groupService;

    [HttpGet("Itself")]
    public async Task<IActionResult> GetItself(int groupId, bool full)
    {
        if (groupId < 0)
            return BadRequest("Incorrect groupId");

        var result = await groupService.GetItSelf(groupId, full);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Students")]
    public async Task<IActionResult> GetStudents(int groupId)
    {
        if (groupId < 0)
            return BadRequest("Incorrect groupId");

        var result = await groupService.GetStudents(groupId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(string name)
    {
        if (name.Length < 4)
            return BadRequest("Name is short");

        var result = await groupService.Create(name);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("Add/One")]
    public async Task<IActionResult> Create(int groupId, int studentId)
    {
        if (groupId < 0)
            return BadRequest("Incorrect groupId");

        var result = await groupService.AddStudentInGroup(groupId, studentId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("Add/Many")]
    public async Task<IActionResult> Create(int groupId, int[] studentsId)
    {
        if (groupId < 0)
            return BadRequest("Incorrect groupId");

        var result = await groupService.AddStudentsInGroup(groupId, studentsId);
        return StatusCode(result.StatusCode, result);
    }
}
