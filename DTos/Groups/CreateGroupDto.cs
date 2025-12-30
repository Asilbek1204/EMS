namespace EMS.Api.DTOs.Groups;

public class CreateGroupDto
{
    public string Name { get; set; } = null!;
    public int MentorId { get; set; }
    public decimal Price { get; set; }
}
