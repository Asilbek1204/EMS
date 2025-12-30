namespace EMS.Api.DTos.Students
{
    public class StudentReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
    }
}
