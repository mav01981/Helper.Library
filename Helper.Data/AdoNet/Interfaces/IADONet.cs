using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

public interface IADONetContext
{
    Task<bool> ExecuteNonQueryAsync<T>(string sql, SqlParameter[] sqlParameters = null)
        where T : class, new();

    Task<T> ExecuteReaderSingleAsync<T>(string sql, SqlParameter[] sqlParameters = null)
        where T : class, new();

    Task<List<T>> ExecuteReaderCollectionAsync<T>(string sql, SqlParameter[] sqlParameters = null)
        where T : class, new();
}