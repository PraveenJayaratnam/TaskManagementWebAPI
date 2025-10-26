namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
