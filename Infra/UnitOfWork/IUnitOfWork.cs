using System;
using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        int SaveChanges();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork 
        where TContext : DbContext
    {
        TContext Context { get; }
    }
}
