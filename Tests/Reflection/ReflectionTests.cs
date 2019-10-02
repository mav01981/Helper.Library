﻿using Helper.Reflection;
using Tests.Model;
using Xunit;

namespace Tests
{
    public class ReflectionTests
    {
        private Person CreatSut()
        {
            return
                new Person
                {
                    Name = "John Doe",
                    Age = 21,
                    AddressDetail = new Address
                    {
                        Number = 1,
                        Street = "First St.",
                        CountryDetail = new Country
                        {
                            CountryName = "USA"
                        }
                    }
                };
        }

        [Fact]
        public void GetPropertyDisplayName()
        {
            //Arrange
            var person = CreatSut();
            //Act
            var result = PropertyReflection
                .GetPropertyDisplayName<Person>(x => x.AddressDetail.CountryDetail.CountryName);
            //Assert
            Assert.Equal("AddressDetail.CountryDetail.CountryName", result);
        }

        [Fact]
        public void GetPropertyValue()
        {
            //Arrange
            var person = CreatSut();
            //Act
            var result = PropertyReflection.GetPropertyValue<Person>(person,
                x => x.AddressDetail.CountryDetail.CountryName);
            //Assert
            Assert.Equal("USA", result);
        }

        [Fact]
        public void UpdateNestedStringProperty()
        {
            //Arrange
            var person = CreatSut();
            //Act     
            PropertyReflection.SetProperty("AddressDetail.CountryDetail.CountryName", person, "SWEDEN");
            //Assert
            Assert.Equal("SWEDEN", person.AddressDetail.CountryDetail.CountryName);
        }

        [Fact]
        public void UpdateSingularIntProperty()
        {
            //Arrange
            var person = CreatSut();
            //Act     
            PropertyReflection.SetProperty("Age", person, 25);
            //Assert
            Assert.Equal(25, person.Age);
        }

        [Fact]
        public void UpdateNestedIntProperty()
        {
            //Arrange
            var person = CreatSut();
            //Act     
            PropertyReflection.SetProperty("AddressDetail.Number", person, 4);
            //Assert
            Assert.Equal(4, person.AddressDetail.Number);
        }
    }
}
