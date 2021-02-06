using System;
using System.Data;
using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class;
        IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class;
        IRepositoryDapper<TEntity> GetRepositoryDapper<TEntity>() where TEntity : class;
        void ForceBeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        int SaveChanges();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork 
        where TContext : DbContext
    {
        TContext Context { get; }
    }
}
