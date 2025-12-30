namespace EMS.Api.Entities;
public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public int MentorId { get; set; }
    public User Mentor { get; set; } = null!;

    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
    public ICollection<UserGroup> UserGroups { get; set; }
}
