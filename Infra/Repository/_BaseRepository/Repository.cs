using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository._BaseRepository
{
    public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        public Repository(DbContext context) : base(context)
        {
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);

        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
