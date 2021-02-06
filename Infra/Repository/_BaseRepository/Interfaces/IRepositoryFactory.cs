﻿namespace Infra.Repository._BaseRepository.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
        IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class;
        IRepositoryReadOnly<T> GetReadOnlyRepository<T>() where T : class;
    }
}
