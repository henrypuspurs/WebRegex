using System.Collections.Generic;

namespace WebRegex.Data
{
    public interface ISqlData
    {
        void GetConnectionString(string ConnectionString);
        int SqlExecute<T>(string sql, T data);
        List<T> SqlQuery<T>(string sql);
    }
}