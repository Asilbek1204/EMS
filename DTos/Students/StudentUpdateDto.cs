namespace EMS.Api.DTos.Students;

public class StudentUpdateDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
}
