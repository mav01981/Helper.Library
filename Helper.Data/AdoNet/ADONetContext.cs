using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public class ADONetContext : IADONetContext
{
    private readonly IADOConnectionFactory adoConnectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdoNet"/> class.
    /// </summary>
    /// <param name="connectionString">Connection String for Ado.net connection.</param>
    public ADONetContext(IADOConnectionFactory adoConnectionFactory,  string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Please provide a connection string.", nameof(connectionString));
        }

        this.adoConnectionFactory = adoConnectionFactory;
        this.ConnectionString = connectionString;
    }

    private string ConnectionString { get; }

    public async Task<bool> ExecuteNonQueryAsync<T>(string sql, CancellationToken cancelationToken, SqlParameter[] sqlParameters = null)
        where T : class, new()
    {
        bool result = false;

        using (SqlConnection connection = this.adoConnectionFactory.CreateConnection(this.ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand(sql, connection);

            if (sqlParameters == null)
            {
                sqlCommand.CommandType = CommandType.Text;
            }
            else
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                foreach (var param in sqlParameters)
                {
                    sqlCommand.Parameters.AddWithValue(param.ParameterName, param.Value);
                }
            }

            result = await sqlCommand.ExecuteNonQueryAsync(cancelationToken) == 1;
        }

        return result;
    }

    public async Task<T> ExecuteReaderSingleAsync<T>(string sql, SqlParameter[] sqlParameters = null)
        where T : class, new()
    {
        var @object = new T();

        using (SqlConnection connection = this.adoConnectionFactory.CreateConnection(this.ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand(sql, connection);

            if (sqlParameters == null)
            {
                sqlCommand.CommandType = CommandType.Text;
            }
            else
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                foreach (var param in sqlParameters)
                {
                    sqlCommand.Parameters.AddWithValue(param.ParameterName, param.Value);
                }
            }

            using (var dataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.Default))
            {
                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        @object = this.MapDataToObject(dataReader, new T());
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
        using (SqlConnection connection = this.adoConnectionFactory.CreateConnection(this.ConnectionString))
        {
            SqlCommand sqlCommand = new SqlCommand(sql, connection);

            if (sqlParameters == null)
            {
                sqlCommand.CommandType = CommandType.Text;
            }
            else
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                foreach (var param in sqlParameters)
                {
                    sqlCommand.Parameters.AddWithValue(param.ParameterName, param.Value);
                }
            }

            using (var dataReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.Default))
            {
                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        newListObject.Add(this.MapDataToObject(dataReader, new T()));
                    }
                }
            }
        }

        return newListObject;
    }

    private T MapDataToObject<T>(SqlDataReader dataReader, T newObject)
    {
        if (newObject == null)
        {
            throw new ArgumentNullException(nameof(newObject));
        }

        var objectMemberAccessor = newObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
        var propertiesHashSet = objectMemberAccessor
                .Select(mp => mp.Name)
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        for (int i = 0; i < dataReader.FieldCount; i++)
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
