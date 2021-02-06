using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Options;

namespace Infra.Dapper
{
    public partial class DapperBaseConnection 
    {

        private readonly DapperOptions _options;

        public  IDbConnection Connection;

         public DapperBaseConnection(IOptions<DapperOptions> optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            Type type = typeof(SqlConnection);
            Connection = Activator.CreateInstance(type) as IDbConnection;
            Connection.ConnectionString = _options.ConnectionString;
        }

    }
}