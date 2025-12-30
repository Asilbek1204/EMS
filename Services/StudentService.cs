using EMS.Api.Data;
using EMS.Api.DTos.Students;
using EMS.Api.Entities;
using EMS.Api.Enums;
using EMS.Api.Services.Intrfaces;
using Microsoft.EntityFrameworkCore;

namespace EMS.Api.Services;

public class StudentService(AppDbContext context) : IStudentService
{
    public async Task<StudentReadDto> CreateAsync(StudentCreateDto dto)
    {
        var student = new Student
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender.ToLower() == "male" ? Gender.Male : Gender.Female
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();

        return await GetByIdAsync(student.Id);
    }

    public async Task<StudentReadDto> UpdateAsync(StudentUpdateDto dto)
    {
        var student = await context.Students.FindAsync(dto.Id);
        if (student == null) throw new Exception("Student not found");

        student.FullName = dto.FullName;
        student.PhoneNumber = dto.PhoneNumber;
        student.DateOfBirth = dto.DateOfBirth;
        student.Gender = dto.Gender.ToLower() == "male" ? Gender.Male : Gender.Female;

        await context.SaveChangesAsync();
        return await GetByIdAsync(student.Id);
    }

    public async Task<List<StudentReadDto>> GetAllAsync()
    {
        var students = await context.Students
            .Include(s => s.StudentGroups)
            .ThenInclude(sg => sg.Group)
            .ThenInclude(g => g.Mentor)
            .ToListAsync();

        return students.Select(s => new StudentReadDto
        {
            Id = s.Id,
            FullName = s.FullName,
            PhoneNumber = s.PhoneNumber,
            DateOfBirth = s.DateOfBirth,
            Gender = s.Gender.ToString(),
            Groups = s.StudentGroups.Select(sg => new GroupDto
            {
                Id = sg.GroupId,
                Name = sg.Group.Name,
                Price = sg.Group.Price,
                Mentor = $"{sg.Group.Mentor.FirstName} {sg.Group.Mentor.LastName}"
            }).ToList()
        }).ToList();
    }

    public async Task<StudentReadDto> GetByIdAsync(int id)
    {
        var student = await context.Students
            .Include(s => s.StudentGroups)
            .ThenInclude(sg => sg.Group)
            .ThenInclude(g => g.Mentor)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null) throw new Exception("Student not found");

        return new StudentReadDto
        {
            Id = student.Id,
            FullName = student.FullName,
            PhoneNumber = student.PhoneNumber,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender.ToString(),
            Groups = student.StudentGroups.Select(sg => new GroupDto
            {
                Id = sg.GroupId,
                Name = sg.Group.Name,
                Price = sg.Group.Price,
                Mentor = $"{sg.Group.Mentor.FirstName} {sg.Group.Mentor.LastName}"
            }).ToList()
        };
    }

    public async Task AddToGroupAsync(int studentId, int groupId)
    {
        var exists = await context.StudentGroups
            .AnyAsync(sg => sg.StudentId == studentId && sg.GroupId == groupId);

        if (exists) return;

        context.StudentGroups.Add(new StudentGroup
        {
            StudentId = studentId,
            GroupId = groupId,
            JoinedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }

    public async Task RemoveFromGroupAsync(int studentId, int groupId)
    {
        var studentGroup = await context.StudentGroups
            .FirstOrDefaultAsync(sg => sg.StudentId == studentId && sg.GroupId == groupId);

        if (studentGroup == null) return;

        context.StudentGroups.Remove(studentGroup);
        await context.SaveChangesAsync();
    }
}
