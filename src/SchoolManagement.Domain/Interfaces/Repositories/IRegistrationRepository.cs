using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Interfaces.Repositories;

public interface IRegistrationRepository
{
    Task<RegistrationEntity> AddRegistrationStudent(RegistrationEntity registration);
    Task<IEnumerable<StudentEntity>> GetStudentsByClass(Guid classId);
    Task<int> GetStudentCountByClass(Guid classId);
}
