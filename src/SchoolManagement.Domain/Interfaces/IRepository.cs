using SchoolManagement.Domain.Common;
using System.Linq.Expressions;

namespace SchoolManagement.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task Add(TEntity entity);
    Task<TEntity?> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<PaginatedResponse<TEntity>> GetAllPaged(int pageNumber, int pageSize);
    Task<PaginatedResponse<TEntity>> GetFilteredAsync(List<Expression<Func<TEntity, bool>>> filters, int pageNumber, int pageSize);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
}
