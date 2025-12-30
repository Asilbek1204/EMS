namespace EMS.Api.DTOs.Groups;

public class GroupListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Mentor { get; set; } = null!;
    public int Students { get; set; }
}
