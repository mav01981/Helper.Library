using System.Collections.Generic;
using System.Linq;

public static class PersonData
{
    public static IEnumerable<object[]> TestData(int numTests)
    {
        return new List<object[]>
        {
             new object[] { 1, "James Smith", 21 },
             new object[] { 2, "John Smith", 45 },
             new object[] { 3, "Claire Smith", 12 },
             new object[] { 4, "Robert Smith", 65 },
        }
        .Take(numTests);
    }
}


