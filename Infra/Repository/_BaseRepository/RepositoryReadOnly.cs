using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository._BaseRepository
{
    public class RepositoryReadOnly<T> : BaseRepository<T>, IRepositoryReadOnly<T> where T : class
    {
        public RepositoryReadOnly(DbContext context) : base(context)
        {
        }
    }
}
