using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Domain.Interfaces.Repositories;
using SchoolManagement.Domain.Notifications;
using SchoolManagement.Infrastructure.Context;

namespace SchoolManagement.Infrastructure.Repositories;

public class ClassRepository : Repository<ClassEntity>, IClassRepository
{
    public ClassRepository(DataContext context, INotifier notifier)
        : base(context, notifier) { }

    public override async Task<PaginatedResponse<ClassEntity>> GetAllPaged(int pageNumber, int pageSize)
    {
        try
        {
            _notifier.Handle($"Getting all {typeof(ClassEntity).Name} ordered by ClassName.");

            var totalCount = await _dbSet.CountAsync();
            var query = _dbSet
                .OrderBy(c => c.ClassName)
                .ThenBy(c => c.Id);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<ClassEntity>(items, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting all {typeof(ClassEntity).Name}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}