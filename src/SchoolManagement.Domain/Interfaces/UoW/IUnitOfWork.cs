using Microsoft.EntityFrameworkCore.Storage;
using SchoolManagement.Domain.Interfaces.Repositories;

namespace SchoolManagement.Domain.Interfaces.UoW;

public interface IUnitOfWork : IDisposable
{
    IStudentRepository Students { get; }
    IClassRepository Classes { get; }
    IRegistrationRepository Registrations { get; }
    Task<int> SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
