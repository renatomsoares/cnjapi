namespace Infra.Dapper
{
    public class DapperOptions
    {
        public string ConnectionString {get;set;}

        public DatabaseType DatabaseType {get;set;} = DatabaseType.SqlServer;
    }

    public enum DatabaseType{
        SqlServer,
        MySql,
        SqLite
    }
}