using Microsoft.EntityFrameworkCore.Storage;
using SchoolManagement.Domain.Interfaces.Repositories;
using SchoolManagement.Domain.Interfaces.UoW;
using SchoolManagement.Infrastructure.Context;

namespace SchoolManagement.Infrastructure.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    private IDbContextTransaction _transaction;

    public IStudentRepository Students { get; }
    public IClassRepository Classes { get; }
    public IRegistrationRepository Registrations { get; }

    public UnitOfWork(DataContext dataContext,
        IStudentRepository studentRepository,
        IClassRepository classRepository,
        IRegistrationRepository registrationRepository)
    {
        _dataContext = dataContext;
        Students = studentRepository;
        Classes = classRepository;
        Registrations = registrationRepository;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await _dataContext.Database.BeginTransactionAsync();
        }
        return _transaction;
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveAsync()
    {
        return await _dataContext.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dataContext.Dispose();
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
