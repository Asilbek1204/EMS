using EMS.Api.Data;
using EMS.Api.DTOs.Auth;
using EMS.Api.Helpers.Exceptions;
using EMS.Api.Helpers.JwtHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db, JwtTokenGenerator jwt) : ControllerBase
{

    [HttpPost("login")]
    public IActionResult Login(LoginRequestDto dto)
    {
        var user = db.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefault(u => u.UserName == dto.Username);

        if (user == null)
            throw new UnauthorizedException("Username or password incorrect");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized("Incorrect password");
        }

        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .ToList();

        var token = jwt.Generate(user, roles);

        return Ok(new
        {
            token,
            FullName = $"{user.FirstName} {user.LastName}"
        });
    }
}

