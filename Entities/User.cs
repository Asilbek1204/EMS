using System.Text.Json.Serialization;

namespace EMS.Api.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserGroup> UserGroups { get; set; }
}
