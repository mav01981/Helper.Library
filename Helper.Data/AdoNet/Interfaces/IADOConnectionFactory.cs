using System.Data;

public interface IADOConnectionFactory
{
    IDbConnection CreateConnection(string connectionString);
}