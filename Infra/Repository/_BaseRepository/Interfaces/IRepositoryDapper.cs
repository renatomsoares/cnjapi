using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infra.Repository._BaseRepository.Interfaces
{
    public interface IRepositoryDapper<T> where T : class
    {
        IEnumerable<T> GetData(string qry, object param = null);
        int Execute(string qry);

        IEnumerable<TParent> QueryParentChild<TParent, TChild, TParentKey>(
            string sql,
            Func<TParent, TParentKey> parentKeySelector,
            Func<TParent, TChild> childSelector,
            string splitOnTeste = "Id",
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null);
    }
}
