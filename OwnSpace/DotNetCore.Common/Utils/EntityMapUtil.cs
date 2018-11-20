using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DotNetCore.Common.Utils
{
    public class EntityMapUtil
    {
        public static DataTable FromObject<T>(IList<T> list)
        {
            var type = typeof(T);
            var dt = new DataTable();
            var propertyInfoList =
                type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in propertyInfoList)
            {
                dt.Columns.Add(property.Name);
            }
            foreach (var model in list)
            {
                var row = dt.NewRow();
                dt.Rows.Add(row);
                foreach (var property in propertyInfoList)
                {
                    row[property.Name] = property.GetValue(model, null);
                }
            }

            return dt;
        }
    }
}
