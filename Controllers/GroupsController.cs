using EMS.Api.Data;
using EMS.Api.DTos.Groups;
using EMS.Api.DTOs.Groups;
using EMS.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Api.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupsController : ControllerBase
{
    private readonly AppDbContext _db;

    public GroupsController(AppDbContext db)
    {
        _db = db;
    }

    // CREATE — Admin
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateGroupDto dto)
    {
        var group = new Group
        {
            Name = dto.Name,
            Price = dto.Price,
            MentorId = dto.MentorId
        };

        _db.Groups.Add(group);
        await _db.SaveChangesAsync();

        return Ok();
    }

    // UPDATE — Admin
    [HttpPut]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(UpdateGroupDto dto)
    {
        var group = await _db.Groups.FindAsync(dto.Id);
        if (group == null) return NotFound();

        group.Name = dto.Name;
        group.Price = dto.Price;
        group.MentorId = dto.MentorId;

        await _db.SaveChangesAsync();
        return Ok();
    }

    // GET BY ID — Admin, Teacher
    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<GroupDetailsDto>> Get(int id)
    {
        var group = await _db.Groups
            .Include(g => g.Mentor)
            .Include(g => g.StudentGroups)
                .ThenInclude(sg => sg.Student)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null) return NotFound();

        return new GroupDetailsDto
        {
            Id = group.Id,
            Name = group.Name,
            Price = group.Price,
            Mentor = $"{group.Mentor.FirstName} {group.Mentor.LastName}",
            Students = group.StudentGroups.Select(sg => new GroupStudentDto
            {
                Id = sg.Student.Id,
                FullName = sg.Student.FullName,
                JoinedAt = sg.JoinedAt
            }).ToList()
        };
    }

    // GET LIST — Admin, Teacher
    [HttpGet]
    //[Authorize(Roles = "Admin,Teacher")]
    public async Task<List<GroupListDto>> GetList()
    {
        return await _db.Groups
            .Include(g => g.Mentor)
            .Include(g => g.StudentGroups)
            .Select(g => new GroupListDto
            {
                Id = g.Id,
                Name = g.Name,
                Price = g.Price,
                Mentor = g.Mentor.FirstName + " " + g.Mentor.LastName,
                Students = g.StudentGroups.Count
            })
            .ToListAsync();
    }
}
