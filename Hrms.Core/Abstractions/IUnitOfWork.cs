namespace Hrms.Core.Abstractions
{
    public interface IUnitOfWork
    {
        void BeginTransaction();

        Task BeginTransactionAsync();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Commit();

        Task CommitAsync();

        void Rollback();

        Task RollbackAsync();

        int ExecuteSqlRaw(string sql, params object[] parameters);

        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    }
}
