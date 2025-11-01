using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Interfaces.Services;

public interface IClassService
{
    Task<ClassEntity?> GetByIdAsync(Guid classId);
    Task<IEnumerable<ClassEntity>> GetAllAsync();
    Task<bool> CreateBoxAsync(ClassEntity classe, string userEmail);
    Task<bool> UpdateBoxAsync(ClassEntity classe, string userEmail);
    Task<bool> SoftDeleteBoxAsync(Guid classId, string userEmail);
}
