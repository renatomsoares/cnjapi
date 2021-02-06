using Infra.Dapper;
using Infra.Repository._BaseRepository;
using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Xml.Schema;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infra.UnitOfWork
{
    public class UnitOfWork<TContext> :
            IRepositoryFactory,
            IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        private Dictionary<Type, object> _repositories;
        public TContext Context { get; }
        private readonly DapperBaseConnection _dapperBaseConnection;
        private IDbContextTransaction _transaction;
        private IsolationLevel? _isolationLevel;
        private readonly IConfiguration _config;

        public UnitOfWork(TContext context, DapperBaseConnection dapperBaseConnection, IConfiguration config)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _dapperBaseConnection = dapperBaseConnection;
            _config = config;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new Repository<TEntity>(Context);
            return (IRepository<TEntity>)_repositories[type];
        }

        public IRepositoryDapper<TEntity> GetRepositoryDapper<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryDapper<TEntity>(_dapperBaseConnection, _config);
            return (IRepositoryDapper<TEntity>)_repositories[type];
        }

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryAsync<TEntity>(Context);
            return (IRepositoryAsync<TEntity>)_repositories[type];
        }

        public IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new RepositoryReadOnly<TEntity>(Context);
            return (IRepositoryReadOnly<TEntity>)_repositories[type];
        }
        
        /// <summary>
        /// Defini o nível de isolamento da transação
        /// </summary>
        /// <param name="isolationLevel"></param>
        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            _isolationLevel = isolationLevel;
        }
        
        /// <summary>
        /// Força ao UnitOfWork iniciar uma transação
        /// </summary>
        public void ForceBeginTransaction()
        {
            StartNewTransactionIfNeeded();
        }

        /// <summary>
        /// Realizar o commit caso exista uma transação
        /// </summary>
        /// <param name="tokenInfo"></param>
        public void CommitTransaction()
        {
            SaveChanges();

            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Realiza rollback caso exista uma transação
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// SaveChanges com dados do Token
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
        
        private void StartNewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                if (_isolationLevel.HasValue)
                {
                    _transaction = Context.Database.BeginTransaction(_isolationLevel.GetValueOrDefault());
                }
                else
                {
                    _transaction = Context.Database.BeginTransaction();
                }
            }
        }
    }
}
