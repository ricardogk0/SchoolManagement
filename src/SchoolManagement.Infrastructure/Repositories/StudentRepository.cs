using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Interfaces;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Infrastructure.Context;

namespace SchoolManagement.Infrastructure.Repositories;

public class StudentRepository : Repository<StudentEntity>, IStudentRepository
{
    public StudentRepository(DataContext context, INotifier notifier)
        : base(context, notifier) { }
}
