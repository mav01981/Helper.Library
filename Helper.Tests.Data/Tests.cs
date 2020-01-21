using Helper.Data;
using Helper.Data.Enum;
using Microsoft.EntityFrameworkCore;

using System;
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
            var result = factory.Create(DataSource.Sql, "");
            //Assert
            Assert.True(result.Database.IsSqlServer());
        }
    }
}
