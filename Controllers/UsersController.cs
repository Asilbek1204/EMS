using EMS.Api.Data;
using EMS.Api.DTos.Users;
using EMS.Api.DTOs.Users;
using EMS.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (await db.Users.AnyAsync(x => x.UserName == dto.Username))
            return BadRequest("Username already exists");

        var user = new User
        {
            UserName = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var roles = await db.Roles
            .Where(r => dto.Roles.Contains(r.Name))
            .ToListAsync();

        foreach (var role in roles)
        {
            db.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        await db.SaveChangesAsync();
        return Ok();
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await db.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Select(u => new UserListDto
            {
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FirstName + " " + u.LastName,
                Roles = u.UserRoles
                    .Select(ur => ur.Role.Name)
                    .ToList()
            })
            .ToListAsync();

        return Ok(users);
    }

}
