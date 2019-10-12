using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Helper.Extensions;
using Helper.Reflection;

namespace Helper.CSV
{
    public class CSV : ICSV
    {
        public delegate void LogProgressHandler(string message);

        public LogProgressHandler logger; 

        /// <summary>
        /// Enable to remove column name from export.
        /// </summary>
        public bool ExportWitOutHeader { get; set; }
        /// <summary>
        /// Function to export data to a csv file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public async Task<bool> ExportToCSV<T>(IEnumerable<T> data, string filePath, params Expression<Func<T, object>>[] columns)
        {
            DataTable dataTable = this.ConvertToDataTable(data, columns);

            dataTable.ToCSV(filePath);

            return await Task.FromResult(true);
        }
        /// <summary>
        /// Import csv file into a datatable.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="split"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Import csv file int a collection of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public ICollection<T> Import<T>(string file, char split, bool hasHeader) where T : new()
        {
            var list = new List<T>();
            var fileContents = File.ReadAllLines(file);

            Dictionary<string, object> columns = new Dictionary<string, object>();

            for (int i = 0; i < fileContents.Length; i++)
            {
                if (i == 0 && hasHeader)
                {
                    columns = MapHeaderFromFile(fileContents[i].ToString());
                    continue;
                }
                else if (i == 0)
                {
                    columns = MapHeaderFromObject<T>(fileContents[i].ToString());
                    continue;
                }

                var dict = MapValues(columns, fileContents[i].ToString());

                list.Add(Mapper.MapDictionaryToObject<T>(dict));

                logger.Invoke($"{i}");
            }

            return list;
        }


        /// <summary>
        /// Import csv file int a collection of T 
        /// provide mapped column headers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="split"></param>
        /// <param name="columnHeaders"></param>
        /// <returns></returns>
        public ICollection<T> Import<T>(string file, char split, string[] columnHeaders) where T : new()
        {
            var list = new List<T>();
            var fileContents = File.ReadAllLines(file);

            if (fileContents[0].Split(split).Length != columnHeaders.Length)
                throw new InvalidOperationException($"Column header {columnHeaders.Length} - Column value mismatch {fileContents[0].Split(split).Length }");

            Dictionary<string, object> columns = new Dictionary<string, object>();

            for (int i = 0; i < fileContents.Length; i++)
            {
                if (i == 0)
                {
                    columns = MapHeaderFromFile(columnHeaders.ToString());
                    continue;
                }

                var dict = MapValues(columns, fileContents[i].ToString());

                list.Add(Mapper.MapDictionaryToObject<T>(dict));

                logger.Invoke($"{i}");
            }

            return list;
        }

        private static Dictionary<string, object> MapHeaderFromFile(string columns)
        {
            return columns.Split(',').ToDictionary(key => key, value => ((object)value));
        }
        private static Dictionary<string, object> MapHeaderFromObject<T>(string columns) where T : new()
        {
            var obj = new T();

            var properties = obj.GetType().GetProperties();

            var header = columns.Split(',');

            GetProperties<T>(obj, new Dictionary<string, object>(), false);

            return columns.Split(',').ToDictionary(key => key, value => ((object)value));
        }

        private static Dictionary<string, object> MapValues(Dictionary<string, object> dictionary, string contents)
        {
            var values = contents.Split(',').ToList();
            var dict = new Dictionary<string, object>();

            int i = 0;
            foreach (var value in dictionary)
            {
                dict.Add(value.Key, values[i]);
                i++;
            }

            return dict;
        }

        private static void GetProperties<T>(object obj,
            Dictionary<string, object> result,
            bool isChild)
        {
            Type objectType = obj.GetType();

            var properties = objectType.GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                if (!properties[i].PropertyType.IsValueType && properties[i].PropertyType != typeof(string))
                {
                    GetProperties<T>(Activator.CreateInstance(properties[i].PropertyType), result, true);
                }
                else
                {
                    result.Add(isChild ?
                        $"{properties[i].ReflectedType.Name}.{properties[i].Name}"
                        : $"{properties[i].Name}", null);

                }
            }
        }

        private DataTable ConvertToDataTable<T>(IEnumerable<T> data,
            params Expression<Func<T, object>>[] columnsFunc)
        {
            DataTable table = new DataTable();

            foreach (var column in columnsFunc)
            {
                string columnName = PropertyReflection.GetPropertyDisplayName<T>(column);
                table.Columns.Add(columnName);
            }

            foreach (T obj in data)
            {
                DataRow row = table.NewRow();

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[table.Columns[i].ColumnName] = PropertyReflection.GetPropertyValue<T>(obj, columnsFunc[i]);
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
