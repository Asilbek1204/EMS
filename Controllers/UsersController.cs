using EMS.Api.Data;
using EMS.Api.DTOs.Users;
using EMS.Api.Entities;
using EMS.Api.Helpers.JwtHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher _hasher;

    public UsersController(AppDbContext db, PasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (await _db.Users.AnyAsync(x => x.UserName == dto.Username))
            return BadRequest("Username already exists");

        var user = new User
        {
            UserName = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PasswordHash = _hasher.Hash(dto.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var roles = await _db.Roles
            .Where(r => dto.Roles.Contains(r.Name))
            .ToListAsync();

        foreach (var role in roles)
        {
            _db.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        await _db.SaveChangesAsync();
        return Ok();
    }
}
