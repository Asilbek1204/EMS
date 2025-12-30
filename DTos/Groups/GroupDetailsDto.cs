namespace EMS.Api.DTos.Groups
{
    public class GroupDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Mentor { get; set; } = null!;
        public List<GroupStudentDto> Students { get; set; } = new();
    }
    public class GroupStudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime JoinedAt { get; set; }
    }
}
