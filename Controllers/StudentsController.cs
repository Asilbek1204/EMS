using EMS.Api.DTos.Students;
using EMS.Api.Services.Intrfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController(IStudentService service) : ControllerBase
{

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        var student = await service.CreateAsync(dto);
        return Ok(student);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(StudentUpdateDto dto)
    {
        var student = await service.UpdateAsync(dto);
        return Ok(student);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        var students = await service.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Get(int id)
    {
        var student = await service.GetByIdAsync(id);
        return Ok(student);
    }

    [HttpPost("add-to-group")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddToGroup([FromBody] AddStudentToGroupDto dto)
    {
        await service.AddToGroupAsync(dto.StudentId, dto.GroupId);
        return Ok();
    }

    [HttpPost("remove-from-group")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> RemoveFromGroup([FromBody] AddStudentToGroupDto dto)
    {
        await service.RemoveFromGroupAsync(dto.StudentId, dto.GroupId);
        return Ok();
    }
}

public class AddStudentToGroupDto
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
}
