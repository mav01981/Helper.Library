using System.Data;
using System.Data.SqlClient;

public class ADOConnectionFactory : IADOConnectionFactory
{
    private readonly string dbConnectionString;

    public ADOConnectionFactory(string dbConnectionString)
    {
        this.dbConnectionString = dbConnectionString ?? throw new System.ArgumentNullException(nameof(dbConnectionString));
    }

    /// <inheritdoc/>
    public IDbConnection CreateConnection(string connectionString)
    {
        var connection = new SqlConnection(connectionString);

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return connection;
    }
}