using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebRegex.Data
{
    public class SqlData : ISqlData
    {
        private string _connectionString;

        public void GetConnectionString(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public List<T> SqlQuery<T>(string sql)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>(sql).ToList();
            }
        }

        public int SqlExecute<T>(string sql, T data)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Execute(sql, data);
            }
        }
    }
}
