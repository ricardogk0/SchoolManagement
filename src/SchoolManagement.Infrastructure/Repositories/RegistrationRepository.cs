using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Interfaces;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Infrastructure.Context;

namespace SchoolManagement.Infrastructure.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    protected readonly DataContext _dataContext;
    protected readonly DbSet<RegistrationEntity> _dbSet;
    protected readonly INotifier _notifier;

    public RegistrationRepository(DataContext dataContext, INotifier notifier)
    {
        _dataContext = dataContext;
        _dbSet = dataContext.Set<RegistrationEntity>();
        _notifier = notifier;
    }

    public async Task<RegistrationEntity?> AddRegistrationStudent(RegistrationEntity registration)
    {
        try
        {
            await _dbSet.AddAsync(registration);
            await _dataContext.SaveChangesAsync();

            _notifier.Handle("Registration completed successfully.");
            return registration;
        }
        catch (Exception ex)
        {
            _notifier.Handle(ex);
            throw;
        }
    }

    public async Task<IEnumerable<StudentEntity>> GetStudentsByClass(Guid classId)
    {
        try
        {
            var students = await _dbSet
                .Include(r => r.Student)
                .Include(r => r.Class)
                .Where(r => r.Class.Id == classId && !r.IsDeleted)
                .Select(r => r.Student)
                .ToListAsync();

            _notifier.Handle($"Found {students.Count} registration student(s) in class {classId}.");
            return students;
        }
        catch (Exception ex)
        {
            _notifier.Handle(ex);
            throw;
        }
    }
}
