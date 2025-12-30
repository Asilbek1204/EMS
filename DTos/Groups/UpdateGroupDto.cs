namespace EMS.Api.DTOs.Groups;

public class UpdateGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int MentorId { get; set; }
    public decimal Price { get; set; }
}
