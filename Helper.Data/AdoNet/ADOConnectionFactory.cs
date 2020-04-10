using System.Data;
using System.Data.SqlClient;

public class ADOConnectionFactory : IADOConnectionFactory
{
    private readonly string dbConnectionString;

    public ADOConnectionFactory(string dbConnectionString)
    {
        this.dbConnectionString = dbConnectionString;
    }

    public SqlConnection CreateConnection(string connectionString)
    {
        SqlConnection connection = new SqlConnection(connectionString);

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return connection;
    }
}