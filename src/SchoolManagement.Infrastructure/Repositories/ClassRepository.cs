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

    public override async Task<PaginatedResponse<ClassEntity>> GetAllPaged(Filters filter)
    {
        try
        {
            _notifier.Handle($"Getting all {typeof(ClassEntity).Name} ordered by ClassName.");

            var totalCount = await _dbSet.CountAsync();
            var query = _dbSet
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.ClassName)
                .ThenBy(c => c.Id);

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PaginatedResponse<ClassEntity>(items, totalCount, filter.PageNumber, filter.PageSize);
        }
        catch (Exception ex)
        {
            _notifier.Handle($"Error getting all {typeof(ClassEntity).Name}: {ex.Message}", NotificationType.Error);
            throw;
        }
    }
}