using Helper.Reflection;
using System.Collections.Generic;
using Tests.Model;
using Xunit;

namespace Tests.Reflection
{
    public class MapperTests
    {
        private Person CreatSut()
        {
            return
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
                    }
                };
        }

        [Fact]
        public void OutputObjectAsStringList()
        {
            //Arrange
            var person = CreatSut();
            //Act     
            var result = Mapper.DisplayStringCollection<Person>(person);
            //Assert
            Assert.True(result.Count == 5);
        }
        
        [Fact]
        public void OutputDictionaryToObject()
        {
            //Arrange
            var person = CreatSut();
            //Act     
            var result = Mapper.MapDictionaryToObject<Person>(new Dictionary<string, object>() { });
            //Assert
            Assert.Equal(person, result);
        }
    }
}
