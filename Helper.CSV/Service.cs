using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Helper.Extensions;

namespace Helper.CSV
{
    public class Files
    {
        public void ExportToExcel<T>(IEnumerable<T> data, string filePath, params Expression<Func<T, object>>[] columns)
        {
            DataTable dataTable = this.ConvertToDataTable(data, columns);

            dataTable.ToCSV(filePath);
        }
        public DataTable Import(string file, char split)
        {
            var table = new DataTable();

            var fileContents = File.ReadAllLines(file);

            var splitFileContents = (from f in fileContents select f.Split(split)).ToArray();

            int maxLength = (from s in splitFileContents select s.Count()).Max();

            for (int i = 0; i < maxLength; i++)
            {
                table.Columns.Add();
            }

            foreach (var line in splitFileContents)
            {
                DataRow row = table.NewRow();
                row.ItemArray = (object[])line;
                table.Rows.Add(row);
            }

            return table;
        }
        public T Import<T>(IDictionary<string, object> dictionary) where T : new()
        {
            return Reflection.Mapper.MapToDictionary<T>(dictionary);
        }
        private static IDictionary<string, string> ArrayToDictionary<T>(T obj, string content) where T : new()
        {
            var result = new Dictionary<string, string>();

            var properties = obj.GetType().GetProperties();

            var values = content.Split(',');

            for (var i = 0; i < properties.Length; i++)
            {
                if (i < values.Length)

                    result.Add(properties[i].Name, values[i]);
            }
            return result;
        }
        private DataTable ConvertToDataTable<T>(IEnumerable<T> data, params Expression<Func<T, object>>[] columnsFunc)
        {
            DataTable table = new DataTable();

            foreach (var column in columnsFunc)
            {
                string columnName = Reflection.PropertyReflection.GetPropertyDisplayName<T>(column);
                table.Columns.Add(columnName);
            }

            foreach (T obj in data)
            {
                DataRow row = table.NewRow();

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[table.Columns[i].ColumnName] = Reflection.PropertyReflection.GetPropertyValue<T>(obj, columnsFunc[i]);
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
