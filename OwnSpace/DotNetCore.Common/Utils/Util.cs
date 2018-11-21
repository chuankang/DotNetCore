using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DotNetCore.Common.Utils
{
    public static class Util
    {
        /// <summary>
        /// 集合转DataTable
        /// </summary>
        public static DataTable ListToTable<T>(IEnumerable<T> collections)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());

            var enumerable = collections as T[] ?? collections.ToArray();
            if (!enumerable.Any()) return dt;

            for (var i = 0; i < enumerable.Count(); i++)
            {
                var tempList = new ArrayList();
                foreach (var p in props)
                {
                    object obj = p.GetValue(enumerable.ElementAt(i), null);
                    tempList.Add(obj);
                }

                object[] array = tempList.ToArray();
                dt.LoadDataRow(array, true);
            }

            return dt;
        }

        #region DataTable转集合

        /// <summary>
        /// DataTable转集合
        /// </summary>
        public static IList<T> TableToList<T>(DataTable table)
        {
            if (table == null)
            {
                return new List<T>();
            }

            var rows = new  List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            if (rows == null) return null;
            var list = new List<T>();

            foreach (var row in rows)
            {
                var item = CreateItem<T>(row);
                list.Add(item);
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row == null) return obj;
            obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                try
                {
                    object value = row[column.ColumnName];
                    prop.SetValue(obj,value,null);
                }
                catch (Exception e)
                {
                    //You can log something here     
                    //throw;  
                }
            }

            return obj;
        }

        #endregion
    }
}
