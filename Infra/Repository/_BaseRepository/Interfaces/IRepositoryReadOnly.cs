namespace Infra.Repository._BaseRepository.Interfaces
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T> where T : class
    {

    }
}
