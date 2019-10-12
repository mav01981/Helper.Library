namespace Tests.Model
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public int Number { get; set; }
        public string Street { get; set; }
        public Country Country { get; set; }
    }

    public class Country
    {
        public string CountryName { get; set; }
    }
}