using EMS.Api.Data;
using EMS.Api.DTOs.Roles;
using EMS.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _db;

    public RolesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<RoleDto>> Get()
        => await _db.Roles
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
            .ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Create(RoleDto dto)
    {
        _db.Roles.Add(new Role { Name = dto.Name });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _db.Roles.FindAsync(id);
        if (role == null) return NotFound();

        _db.Roles.Remove(role);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
