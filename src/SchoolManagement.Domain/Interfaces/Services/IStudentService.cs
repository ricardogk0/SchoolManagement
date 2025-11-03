using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Dtos.Response;
using SchoolManagement.Domain.DTOs.Response.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Interfaces.Services;

public interface IStudentService
{
    Task<PaginatedResponse<StudentResponseDto>> GetAllAsync();
    Task<StudentResponseDto?> GetByIdAsync(Guid studentId);
    Task<ResponseModel<StudentResponseDto>> CreateStudentAsync(StudentCreateDto student, string userEmail);
    Task<StudentResponseDto?> UpdateStudentAsync(Guid studentId, StudentUpdateDto student, string userEmail);
    Task<bool> SoftDeleteStudentAsync(Guid studentId, string userEmail);
}
