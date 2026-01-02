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
public class GroupsController(AppDbContext db) : ControllerBase
{

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateGroupDto dto)
    {
        var user = await db.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == dto.MentorId);

        if (user == null)
            return BadRequest(new { message = "User not found" });

        var isMentor = user.UserRoles.Any(r =>
            r.Role.Name == "Teacher" ||
            r.Role.Name == "Manager");

        if (!isMentor)
            return BadRequest(new { message = "User is not a mentor" });

        var group = new Group
        {
            Name = dto.Name,
            Price = dto.Price,
            MentorId = dto.MentorId
        };

        db.Groups.Add(group);
        await db.SaveChangesAsync();

        return Ok(new { id = group.Id });
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(UpdateGroupDto dto)
    {
        var group = await db.Groups.FindAsync(dto.Id);
        if (group == null) return NotFound();
        var mentor = await db.Users.FindAsync(dto.MentorId);
        if (mentor == null) return BadRequest(new { message = "Mentor not found" });

        group.Name = dto.Name;
        group.Price = dto.Price;
        group.MentorId = dto.MentorId;

        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<GroupDetailsDto>> Get(int id)
    {
        var group = await db.Groups
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

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<List<GroupListDto>> GetList()
    {
        return await db.Groups
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
