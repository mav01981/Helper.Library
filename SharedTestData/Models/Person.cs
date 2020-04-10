using System;
using System.Diagnostics.CodeAnalysis;

namespace SharedTestData
{
    public class Person : IEquatable<Person>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(this, other)) return false;
            return Id == other.Id && FullName == other.FullName && Age == other.Age;
        }
    }
}
