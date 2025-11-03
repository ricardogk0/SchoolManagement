using SchoolManagement.Domain.Entities;
using System.Linq.Expressions;

namespace SchoolManagement.Domain.Interfaces.Repositories;

public interface IRegistrationRepository
{
    Task<RegistrationEntity> AddRegistrationStudent(RegistrationEntity registration);
    Task<IEnumerable<StudentEntity>> GetStudentsByClass(Guid classId);
    Task<int> GetStudentCountByClass(Guid classId);
    Task<IEnumerable<RegistrationEntity>> Find(Expression<Func<RegistrationEntity, bool>> predicate);
}
