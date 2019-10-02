using Helper.CSV;
using Helper.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Tests.Model;
using Xunit;

namespace Tests
{
    public class CSVTests
    {

        [Fact]
        public void ReadLine()
        {
            var helper = new Files();

            var columns = new Dictionary<string, object>();

            columns.Add("Name", "John Smith");
            columns.Add("Age", 4);

            var model = helper.Import<Person>(columns);


        }
    }
}
