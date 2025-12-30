using EMS.Api.DTos.Students;

namespace EMS.Api.Services.Intrfaces
{
    public interface IStudentService
    {
        Task<StudentReadDto> CreateAsync(StudentCreateDto dto);
        Task<StudentReadDto> UpdateAsync(StudentUpdateDto dto);
        Task<List<StudentReadDto>> GetAllAsync();
        Task<StudentReadDto> GetByIdAsync(int id);
        Task AddToGroupAsync(int studentId, int groupId);
        Task RemoveFromGroupAsync(int studentId, int groupId);
    }
}
