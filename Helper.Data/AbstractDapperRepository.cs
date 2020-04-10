using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Helper.Data
{
    public abstract class AbstractDapperRepository
    {
        private readonly IDbConnection connection;

        public AbstractDapperRepository(IDbConnection conn)
        {
            this.connection = conn;
        }

        public List<T> Query<T>(string query, object parameters = null)
        {
            try
            {
                return this.connection.Query<T>(query, parameters).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T QuerySingle<T>(string query, object parameters = null)
        {
            try
            {
                return this.connection.QuerySingle<T>(query, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T QueryFirst<T>(string query, object parameters = null)
        {
            try
            {
                return this.connection.QueryFirst<T>(query, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert<T>(string query, T model)
        {
            try
            {
                return this.connection.Execute(query, model, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
