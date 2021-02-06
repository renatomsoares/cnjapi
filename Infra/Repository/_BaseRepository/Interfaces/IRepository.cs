using System;
using System.Collections.Generic;

namespace Infra.Repository._BaseRepository.Interfaces
{
    public interface IRepository<T> : IReadRepository<T>, IDisposable where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
