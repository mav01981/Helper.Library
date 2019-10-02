using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Model
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address AddressDetail { get; set; }
    }

    public class Address
    {
        public int Number { get; set; }
        public string Street { get; set; }
        public Country CountryDetail { get; set; }
    }

    public class Country
    {
        public string CountryName { get; set; }
    }
}
