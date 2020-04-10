using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Helper.CSV
{
    public interface IFile
    {
        void ExportToExcel<T>(IEnumerable<T> data, string filePath, params Expression<Func<T, object>>[] columns);
        DataTable Import(string file, char split);
        T Import<T>(string file, char split);
    }
}