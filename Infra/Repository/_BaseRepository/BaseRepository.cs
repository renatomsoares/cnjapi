using System;
using System.Linq;
using System.Linq.Expressions;
using Infra.Repository._BaseRepository.Interfaces;
using Infra.Repository._BaseRepository.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infra.Repository._BaseRepository
{
    public abstract class BaseRepository<T> : IReadRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _dbContext = context ??
                throw new ArgumentException(nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        /// <summary>
        /// Return only FirstOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).FirstOrDefault();
            return query.FirstOrDefault();
        }

        /// <summary>
        /// GetList using orderby, include, tracking and paginate result
        /// </summary>
        /// <used>
        /// Usado no BaseService em GetALL
        /// </used>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        public IPaginate<T> GetList(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 0,
            bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }

        /// <summary>
        /// Usando find
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Select(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).SingleOrDefault();
            //return _dbContext.Set<T> ().Find (id);
        }

        /// <summary>
        /// GetById - Return using predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        public IPaginate<T> GetById(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 0,
            bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (predicate == null)
                return null;

            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            //Return with predicate
            query = query.Where(predicate);

            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }

        /// <summary>
        /// Detach
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void Detach(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Detached;
        }
    }
}