using SchoolManagement.Domain.Common;
using System.Linq.Expressions;

namespace SchoolManagement.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task Add(TEntity entity);
    Task<TEntity> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<PaginatedResponse<TEntity>> GetAllPaged(int pageNumber, int pageSize);
    Task<PaginatedResponse<TEntity>> GetFilteredAsync(List<Expression<Func<TEntity, bool>>> filters, int pageNumber, int pageSize);
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
}
