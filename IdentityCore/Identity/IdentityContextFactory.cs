using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
        public IdentityContext Create(DataSource dataSource, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

            switch (dataSource)
            {
                case DataSource.Sql:
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case DataSource.Sqllite:
                    optionsBuilder.UseSqlite(connectionString);
                    break;
                case DataSource.Postgresql:
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                default:
                    throw new ArgumentNullException("");
            }

            return new IdentityContext(optionsBuilder.Options);
        }
    }

    public enum DataSource
    {
        Sql,
        Sqllite,
        Postgresql
    }

}
