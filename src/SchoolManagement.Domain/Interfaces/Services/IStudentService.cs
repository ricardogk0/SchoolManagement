using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Interfaces.Services;

public interface IStudentService
{
    Task<StudentEntity?> GetByIdAsync(Guid studentId);
    Task<IEnumerable<StudentEntity>> GetAllAsync();
    Task<bool> CreateStudentAsync(StudentEntity student, string userEmail);
    Task<bool> UpdateStudentAsync(StudentEntity student, string userEmail);
    Task<bool> SoftDeleteStudentAsync(Guid studentId, string userEmail);
}
