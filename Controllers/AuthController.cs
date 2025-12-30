using EMS.Api.Data;
using EMS.Api.DTOs.Auth;
using EMS.Api.Helpers.Exceptions;
using EMS.Api.Helpers.JwtHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext db;
    private readonly JwtTokenGenerator jwt;

    public AuthController(AppDbContext db, JwtTokenGenerator jwt)
    {
        this.db = db;
        this.jwt = jwt;
    }

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
            throw new UnauthorizedException("Username or password incorrect");

        // 🔹 ROLE'LARNI OLAMIZ
        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .ToList();

        // 🔹 TO‘G‘RI METOD
        var token = jwt.Generate(user, roles);

        return Ok(new
        {
            token,
            fullName = $"{user.FirstName} {user.LastName}"
        });
    }
}

