using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebRegex.Data
{
    public class SqlData
    {
        private readonly string ConnectionString;

        public SqlData(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<T> SqlQuery<T>(string sql)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<T>(sql).ToList();
            }
        }

        public int SqlExecute<T>(string sql, T data)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return connection.Execute(sql, data);
            }
        }
    }
}
