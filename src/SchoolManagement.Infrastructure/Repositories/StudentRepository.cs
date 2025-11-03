using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Notifications;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Domain.Interfaces.Repositories;
using SchoolManagement.Infrastructure.Context;

namespace SchoolManagement.Infrastructure.Repositories;

public class StudentRepository : Repository<StudentEntity>, IStudentRepository
{
    public StudentRepository(DataContext context, INotifier notifier)
        : base(context, notifier) { }

    public override async Task<PaginatedResponse<StudentEntity>> GetAllPaged(int pageNumber, int pageSize)
    {
        try
        {
            _notifier.Handle($"Getting all {typeof(StudentEntity).Name} ordered by Name.");

            var totalCount = await _dbSet.CountAsync();
            var query = _dbSet
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Id); 

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<StudentEntity>(items, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting all {typeof(StudentEntity).Name}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}
