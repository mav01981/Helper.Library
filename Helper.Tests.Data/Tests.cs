using Helper.Data;
using Helper.Data.Enum;
using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Helper.Tests.Data
{
    public class Tests
    {
        [Fact]
        public void Create_SQL_Context()
        {
            //Arrange
            var factory = new ContextFactory<DbContext>();
            //Act
            var result = factory.Create(DataSource.Sql, string.Empty);
            //Assert
            Assert.True(result.Database.IsSqlServer());
        }

        [Fact]
        public void Create_Oracle_Context()
        {
            //Arrange
            var factory = new ContextFactory<DbContext>();
            //Act
            var result = factory.Create(DataSource.Oracle, string.Empty);
            //Assert
            Assert.True(result.Database.IsSqlServer());
        }
    }
}
