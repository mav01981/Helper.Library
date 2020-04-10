using System.Data.SqlClient;

public interface IADOConnectionFactory
{
    SqlConnection CreateConnection(string connectionString);
}