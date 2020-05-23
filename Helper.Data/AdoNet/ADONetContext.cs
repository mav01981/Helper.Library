using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

public class ADONetContext : IADONetContext
{
    private string _connectionString { get; }

    private readonly IADOConnectionFactory _adoConnectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdoNet"/> class.
    /// </summary>
    /// <param name="connectionString">Connection String for Ado.net connection.</param>
    public ADONetContext(IADOConnectionFactory adoConnectionFactory, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Please provide a connection string.", nameof(connectionString));
        }

        _adoConnectionFactory = adoConnectionFactory;
        _connectionString = connectionString;
    }

    /// <inheritdoc/>
    public async Task<bool> ExecuteNonQueryAsync<T>(string sql, SqlParameter[] sqlParameters = null)
        where T : class, new()
    {
        bool result = false;

        using (IDbConnection connection = _adoConnectionFactory.CreateConnection(_connectionString))
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                if (sqlParameters == null)
                {
                    command.CommandType = CommandType.Text;
                }
                else
                {
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var param in sqlParameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                result = command.ExecuteNonQuery() >= 1;
            }
        }

        return result;
    }

    public async Task<T> ExecuteReaderSingleAsync<T>(string sql, SqlParameter[] sqlParameters = null)
        where T : class, new()
    {
        var @object = new T();

        using (IDbConnection connection = _adoConnectionFactory.CreateConnection(_connectionString))
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandType = sqlParameters == null ? CommandType.Text : CommandType.StoredProcedure;

                if (command.CommandType == CommandType.StoredProcedure)
                {
                    foreach (var param in sqlParameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        @object = this.MapDataToObject(reader, new T());
                    }
                }
            }
        }

        return @object;
    }

    public async Task<List<T>> ExecuteReaderCollectionAsync<T>(string sql, SqlParameter[] sqlParameters = null)
        where T : class, new()
    {
        var newListObject = new List<T>();
        using (IDbConnection connection = _adoConnectionFactory.CreateConnection(_connectionString))
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandType = sqlParameters == null ? CommandType.Text : CommandType.StoredProcedure;

                if (command.CommandType == CommandType.StoredProcedure)
                {
                    foreach (var param in sqlParameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        newListObject.Add(this.MapDataToObject(reader, new T()));
                    }
                }
            }
        }

        return newListObject;
    }

    private T MapDataToObject<T>(IDataReader dataReader, T newObject)
    {
        if (newObject == null)
        {
            throw new ArgumentNullException(nameof(newObject));
        }

        var objectMemberAccessor = newObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
        var propertiesHashSet = objectMemberAccessor
                .Select(mp => mp.Name)
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        for (int i = 0; i <= dataReader.FieldCount; i++)
        {
            var name = propertiesHashSet.FirstOrDefault(a => a.Equals(dataReader.GetName(i), StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrEmpty(name))
            {
                var property = newObject.GetType().GetProperty(name);
                property.SetValue(newObject, dataReader.IsDBNull(i) ? null : dataReader.GetValue(i));
            }
        }

        return newObject;
    }
}
