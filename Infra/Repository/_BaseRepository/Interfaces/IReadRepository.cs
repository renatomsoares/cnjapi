using System;
using System.Linq;
using System.Linq.Expressions;
using Infra.Repository._BaseRepository.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace Infra.Repository._BaseRepository.Interfaces
{
    public interface IReadRepository<T> where T : class
    {

        IQueryable<T> Query(string sql, params object[] parameters);

        T Single(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true);

        IPaginate<T> GetList(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 0,
            bool disableTracking = true);

        IPaginate<T> GetById(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 0,
            bool disableTracking = true);

        T Select(Expression<Func<T, bool>> predicate);

        void Detach(T obj);


    }
}