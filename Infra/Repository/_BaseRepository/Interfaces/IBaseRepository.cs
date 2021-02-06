using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra.Repository._BaseRepository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetAll();
        IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    }
}
