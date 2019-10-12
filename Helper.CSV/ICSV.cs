using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Helper.CSV
{
    public interface ICSV
    {
        Task<bool> ExportToCSV<T>(IEnumerable<T> data, string filePath, params Expression<Func<T, object>>[] columns);
        DataTable Import(string file, char split);
        ICollection<T> Import<T>(string file, char split,bool hasHeade) where T : new();
    }
}