using Hrms.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hrms.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        private IDbContextTransaction _dbTransaction;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void BeginTransaction()
        {
            _dbTransaction = _dataContext.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            _dbTransaction = await _dataContext.Database.BeginTransactionAsync();
        }

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }

        public async Task CommitAsync()
        {
            await _dbTransaction.CommitAsync();
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            await _dbTransaction.RollbackAsync();
        }

        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return _dataContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await _dataContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}
