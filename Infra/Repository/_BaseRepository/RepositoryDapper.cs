using Dapper;
using Infra.Dapper;
using Infra.Repository._BaseRepository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infra.Repository._BaseRepository
{
    public class RepositoryDapper<T> : IRepositoryDapper<T> where T : class
    {
        private readonly DapperBaseConnection _dapperBaseConnection;
        private readonly IConfiguration _config;

        public RepositoryDapper(DapperBaseConnection dapperBaseConnection, IConfiguration config)
        {
            _dapperBaseConnection = dapperBaseConnection;
            _config = config;
        }

        public IDbConnection Connection => new SqlConnection(_config.GetConnectionString("CNJDB_Lawsuits"));


        /// <summary>
        ///  Query using Dapper
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public IEnumerable<T> GetData(string qry, object param = null)
        {
            using (var conexao = Connection)
            {
                conexao.Open();
                var multi = conexao.QueryMultiple(qry, param, commandTimeout: 120);
                var result = multi.Read<T>();
                return result;
            }
        }

        public IEnumerable<TParent> QueryParentChild<TParent, TChild, TParentKey>(
        string sql,
        Func<TParent, TParentKey> parentKeySelector,
        Func<TParent, TChild> childSelector,
        string splitOnTeste = "Id",
        dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var cache = new Dictionary<TParentKey, TParent>();

            using (var conexao = _dapperBaseConnection.Connection)
            {
                var multi = conexao.Query<TParent, TChild, TParent>(
                sql,
                (parent, child) =>
                {
                    if (!cache.ContainsKey(parentKeySelector(parent)))
                    {
                        cache.Add(parentKeySelector(parent), parent);
                    }

                    TParent cachedParent = cache[parentKeySelector(parent)];
                    TChild children = childSelector(cachedParent);

                    if (children != null)
                        children = child;

                    return cachedParent;
                },
                splitOn: splitOnTeste);

                return cache.Values;

            }
        }

        /// <summary>
        ///  Execute Query using Dapper
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public int Execute(string qry)
        {
            using (var conexao = Connection)
            {
                conexao.Open();
                var result = conexao.Execute(qry, commandTimeout: 120);
                return result;
            }
        }

    }
}
