using Microsoft.AspNetCore.Identity;

namespace EMS.Api.Helpers.JwtHelper;

public class PasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
        => _hasher.HashPassword(null!, password);

    public bool Verify(string hash, string password)
        => _hasher.VerifyHashedPassword(null!, hash, password)
           == PasswordVerificationResult.Success;
}
