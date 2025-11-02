using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Dtos.Request;
using SchoolManagement.Domain.Dtos.Response;
using SchoolManagement.Domain.DTOs.Response.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Interfaces.Services;

public interface IStudentService
{
    Task<StudentEntity?> GetByIdAsync(Guid studentId);
    Task<PaginatedResponse<StudentResponseDto>> GetAllAsync();
    Task<ResponseModel<StudentResponseDto>> CreateStudentAsync(StudentCreateDto student, string userEmail);
    Task<bool> UpdateStudentAsync(StudentEntity student, string userEmail);
    Task<bool> SoftDeleteStudentAsync(Guid studentId, string userEmail);
}
