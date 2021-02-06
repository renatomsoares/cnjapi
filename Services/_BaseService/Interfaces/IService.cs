using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Query;

namespace Services._BaseService.Interfaces
{
    public interface IService<T>
    {
        T Add<V>(T obj, string ruleSet) where V : AbstractValidator<T>;
        T Update<V>(T obj, string ruleSet) where V : AbstractValidator<T>;
        void Delete(T entity);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 0,
            bool disableTracking = true
        );
        T GetById(Expression<Func<T, bool>> predicate);
    }
}
