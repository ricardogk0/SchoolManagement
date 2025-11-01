using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Interfaces;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Infrastructure.Context;

namespace SchoolManagement.Infrastructure.Repositories;

public class ClassRepository : Repository<ClassEntity>, IClassRepository
{
    public ClassRepository(DataContext context, INotifier notifier)
        : base(context, notifier) { }
}
