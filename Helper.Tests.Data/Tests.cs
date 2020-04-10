using Helper.Data;
using Helper.Data.Enum;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Helper.Tests.Data
{
    public class Tests
    {
        [Fact(DisplayName = "Create SQL Context.")]
        public void Create_SQL_Context()
        {
            //Arrange
            var factory = new ContextFactory<DbContext>();
            //Act
            var result = factory.Create(DataSource.Sql, $"Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;");
            //Assert
            Assert.True(result.Database.IsSqlServer());
        }

        [Fact(DisplayName = "Create PostgreSQL Context.")]
        public void Create_PostgreSQL_Context()
        {
            //Arrange
            var factory = new ContextFactory<DbContext>();
            //Act
            var result = factory.Create(DataSource.Postgresql, $"Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;");
            //Assert
            Assert.True(result.Database.IsNpgsql());
        }

        [Fact(DisplayName = "Create SQLLite Context.")]
        public void Create_SQLLite_Context()
        {
            //Arrange
            var factory = new ContextFactory<DbContext>();
            //Act
            var result = factory.Create(DataSource.Sqllite, $"Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;");
            //Assert
            Assert.True(result.Database.IsSqlite());
        }
    }
}
