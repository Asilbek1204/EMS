namespace EMS.Api.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string FullName { get; set; } = null!;
}
