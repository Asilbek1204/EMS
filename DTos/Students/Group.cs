namespace EMS.Api.DTos.Students
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Mentor { get; set; } = null!;
    }
}
