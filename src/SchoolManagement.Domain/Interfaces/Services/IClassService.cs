using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Dtos.Response;
using SchoolManagement.Domain.DTOs.Response.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Interfaces.Services;

public interface IClassService
{
    Task<PaginatedResponse<ClassResponseDto>> GetAllAsync();
    Task<ClassResponseDto?> GetByIdAsync(Guid classId);
    Task<ResponseModel<ClassResponseDto>> CreateClassAsync(ClassCreateDto classe, string userEmail);
    Task<ClassResponseDto?> UpdateClassAsync(Guid classId, ClassUpdateDto classe, string userEmail);
    Task<bool> SoftDeleteClassAsync(Guid classId, string userEmail);
}
