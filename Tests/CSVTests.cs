using Helper.CSV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Tests.Model;
using Xunit;

namespace Tests
{
    public class CSVTests
    {
        private string _assemblyPath;
        public CSVTests()
        {
            _assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        private IEnumerable<Person> CreatSut()
        {
            File.WriteAllText($@"{_assemblyPath}\files\SampleA.csv", string.Empty);

            return new List<Person>() {
                new Person
                {
                    Name = "John Doe",
                    Age = 21,
                    Address= new Address
                    {
                        Number = 1,
                        Street = "First St.",
                        Country = new Country
                        {
                            CountryName = "USA"
                        }
                    },
                },
                  new Person
                {
                    Name = "John Smith",
                    Age = 41,
                    Address = new Address
                    {
                        Number = 1,
                        Street = "First St.",
                        Country = new Country
                        {
                            CountryName = "USA"
                        }
                    },
                }};
        }

        [Fact]
        public async Task Export_ObjectCollection_ToCSV()
        {
            //Arrange
            var data = CreatSut();
            var helper = new CSV();
            //Act
            var result = await helper.ExportToCSV<Person>(data,
                $@"{_assemblyPath}\files\SampleB.csv",
                x => x.Name,
                x => x.Age,
                x => x.Address.Street,
                x => x.Address.Number);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Import_CSVFileWithHeader_ToObjectCollection()
        {
            //Arrange
            var headerColumns = new string[] { };
            var helper = new CSV();

            helper.logger  += (sender) =>
            {
                Assert.Equal("", sender);
            };
            //Act
            var result = helper
                .Import<Person>($@"{_assemblyPath}\files\SampleB.csv", ',',
                new string[] { "Name", "Age" });

            //Assert
         
        }

        [Fact]
        public void Import_CSVFileWithOutHeader_ToObjectCollection()
        {
            //Todo:

            //Arrange

            //Act

            //Assert
        }

        [Fact]
        public void Import_CSVWithFewerHeaderColumns_ThrowsInvalidOperationException()
        {
            //Arrange
            var headerColumns = new string[] { };
            var helper = new CSV();
            //Act
            var ex = Assert.Throws<InvalidOperationException>(() => helper
                .Import<Person>($@"{_assemblyPath}\files\SampleB.csv", ',',
                new string[] { "Name", "Age" }));
            //Assert
            Assert.Equal(typeof(InvalidOperationException), ex.GetType());
            Assert.Equal("Column header 2 - Column value mismatch 4", ex.Message);
        }
    }
}
