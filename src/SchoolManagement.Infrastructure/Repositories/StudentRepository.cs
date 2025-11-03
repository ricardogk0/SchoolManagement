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

    public override async Task<PaginatedResponse<StudentEntity>> GetAllPaged(Filters filters)
    {
        try
        {
            _notifier.Handle($"Getting all {typeof(StudentEntity).Name} ordered by Name.");

            var totalCount = await _dbSet.CountAsync();
            var query = _dbSet
                .Where(s =>
                    (string.IsNullOrEmpty(filters.studentName) || s.Name == filters.studentName) &&
                    (string.IsNullOrEmpty(filters.studentDocument) || s.DocumentNumber == filters.studentDocument) &&
                    !s.IsDeleted)
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Id); 

            var items = await query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PaginatedResponse<StudentEntity>(items, totalCount, filters.PageNumber, filters.PageSize);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting all {typeof(StudentEntity).Name}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}
