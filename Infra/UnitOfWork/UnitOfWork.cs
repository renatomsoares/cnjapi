using Infra.Repository._BaseRepository;
using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infra.UnitOfWork
{
    public class UnitOfWork<TContext> :
            IRepositoryFactory,
            IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        private Dictionary<Type, object> _repositories;
        public TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(Context);
            return (IRepository<TEntity>)_repositories[type];
        }



        /// <summary>
        /// SaveChanges method if want to save token data.
        /// </summary>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
