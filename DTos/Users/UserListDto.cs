namespace EMS.Api.DTos.Users;

public class UserListDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}
