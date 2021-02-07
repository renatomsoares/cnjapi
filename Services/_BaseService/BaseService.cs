using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore.Query;
using Services._BaseService.Interfaces;

namespace Services._BaseService
{
    public class BaseService<T> : IService<T> where T : class
    {
        private readonly IUnitOfWork _uow;

        public BaseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Add with Fluent Validator
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ruleSet"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public virtual T Add<V>(T obj, string ruleSet = "*") where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>(), ruleSet);
            var repo = _uow.GetRepository<T>();
            repo.Add(obj);

            //Validate
            _uow.SaveChanges();

            return obj;
        }

        /// <summary>
        /// Update with FluentValidator
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ruleSet"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public virtual T Update<V>(T obj, string ruleSet = "*") where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>(), ruleSet);
            var objectUpdate = _uow.GetRepository<T>().Single(x => x == obj);
            if (objectUpdate != null)
                _uow.GetRepository<T>().Update(obj);

            //Validate
            _uow.SaveChanges();

            return obj;
        }

        /// <summary>
        /// Delete by entity
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Not found entity.");

            var repo = _uow.GetRepository<T>();
            repo.Delete(entity);

            //Validate
            _uow.SaveChanges();
        }

        /// <summary>
        /// Get all data.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 0,
            bool disableTracking = true
        )
        {
            //Set repository you will working
            var repo = _uow.GetRepository<T>();
            return repo.GetList(predicate, orderBy, include, index, size, disableTracking).Items.AsEnumerable();
        }

        public virtual T GetById(Expression<Func<T, bool>> predicate)
        {
            //Set repository you will working
            var repo = _uow.GetRepository<T>();
            //return 
            return repo.Select(predicate);
        }

        public void Detach(T obj)
        {
            var repo = _uow.GetRepository<T>();
            repo.Detach(obj);
        }

        private void Validate(T obj, AbstractValidator<T> validator, string ruleSet = "*")
        {
            if (obj == null)
                throw new Exception("No detected registers!");

            validator.ValidateAndThrow(obj, ruleSet);
        }
    }
}