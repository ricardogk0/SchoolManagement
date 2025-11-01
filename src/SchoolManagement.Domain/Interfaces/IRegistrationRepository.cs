using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Interfaces;

public interface IRegistrationRepository
{
    Task<RegistrationEntity?> AddRegistrationStudent(RegistrationEntity registration);
    Task<IEnumerable<StudentEntity>> GetStudentsByClass(Guid classId);
}
