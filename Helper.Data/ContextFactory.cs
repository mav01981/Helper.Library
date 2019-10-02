using Helper.Data.Enum;

using Microsoft.EntityFrameworkCore;

using System;

namespace Helper.Data
{
    public class ContextFactory<T> where T : DbContext
    {
        public T Create(DataSource dataSource, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();

            switch (dataSource)
            {
                case DataSource.Sql:
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case DataSource.Sqllite:
                    optionsBuilder.UseSqlite(connectionString);
                    break;
                case DataSource.Postgresql:
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                default:
                    throw new ArgumentNullException("No datasource provided");
            }

            return (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options);
        }
    }
}