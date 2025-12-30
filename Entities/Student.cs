using EMS.Api.Enums;
using System.Reflection;
using System.Text.Json.Serialization;

namespace EMS.Api.Entities;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}