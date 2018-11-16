using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using  System.Linq.Dynamic;

namespace DZHAPP.Utils
{
    public static class SelectExtension
    {
        //public static IQueryable<TSource2> SelectEx<TSource, TSource2>(this IQueryable<TSource> source, string property) where TSource : class
        //{
        //    ParameterExpression param = Expression.Parameter(typeof(TSource), "c");
        //    PropertyInfo pi = typeof(TSource).GetProperty(property);
        //    MemberExpression selector = Expression.MakeMemberAccess(param, pi);
        //    LambdaExpression le = Expression.Lambda(selector, param);
        //    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "Select", new Type[] { typeof(TSource), pi.PropertyType }, source.Expression, le);
        //    return source.Provider.CreateQuery<TSource2>(resultExp);
        //}

        ///// <summary>
        ///// 根据指定属性名称对序列进行排序
        ///// </summary>
        ///// <typeparam name="TSource">source中的元素的类型</typeparam>
        ///// <param name="source">一个要排序的值序列</param>
        ///// <param name="property">属性名称</param>
        ///// <param name="descending">是否降序</param>
        ///// <returns></returns>
        //public static IQueryable<TSource> OrderByDesc<TSource>(this IQueryable<TSource> source, string property,
        //    bool descending = true) where TSource : class
        //{
        //    ParameterExpression param = Expression.Parameter(typeof(TSource), "c");
        //    PropertyInfo pi = typeof(TSource).GetProperty(property);
        //    MemberExpression selector = Expression.MakeMemberAccess(param, pi);
        //    LambdaExpression le = Expression.Lambda(selector, param);
        //    string methodName = (descending) ? "OrderByDescending" : "OrderBy";
        //    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
        //        new Type[] { typeof(TSource), pi.PropertyType }, source.Expression, le);
        //    return source.Provider.CreateQuery<TSource>(resultExp);
        //}

        /// <summary>
        /// 动态Linq方式实现行转列
        /// </summary>
        /// <param name="list">数据</param>
        /// <param name="DimensionList">维度列</param>
        /// <param name="DynamicColumn">动态列</param>
        /// <returns>行转列后数据</returns>
        public static List<dynamic> DynamicLinq<T>(List<T> list, List<string> DimensionList, string DynamicColumn, out List<string> AllDynamicColumn) where T : class
        {
            //获取所有动态列
            var columnGroup = list.GroupBy(DynamicColumn, "new(it as Vm)") as IEnumerable<IGrouping<dynamic, dynamic>>;
            List<string> AllColumnList = new List<string>();
            foreach (var item in columnGroup)
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    AllColumnList.Add(item.Key);
                }
            }
            AllDynamicColumn = AllColumnList;
            var dictFunc = new Dictionary<string, Func<T, bool>>();
            foreach (var column in AllColumnList)
            {
                var func = DynamicExpression.ParseLambda<T, bool>(string.Format("{0}==\"{1}\"", DynamicColumn, column)).Compile();
                dictFunc[column] = func;
            }
            //获取实体所有属性
            Dictionary<string, PropertyInfo> PropertyInfoDict = new Dictionary<string, PropertyInfo>();
            Type type = typeof(T);
            var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //数值列
            List<string> AllNumberField = new List<string>();
            foreach (var item in propertyInfos)
            {
                PropertyInfoDict[item.Name] = item;
                if (item.PropertyType == typeof(int) || item.PropertyType == typeof(double) || item.PropertyType == typeof(float))
                {
                    AllNumberField.Add(item.Name);
                }
            }
            //分组
            var dataGroup = list.GroupBy(string.Format("new ({0})", string.Join(",", DimensionList)), "new(it as Vm)") as IEnumerable<IGrouping<dynamic, dynamic>>;
            List<dynamic> listResult = new List<dynamic>();
            IDictionary<string, object> itemObj = null;
            T vm2 = default(T);
            foreach (var group in dataGroup)
            {
                itemObj = new ExpandoObject();
                var listVm = group.Select(e => e.Vm as T).ToList();
                //维度列赋值
                vm2 = listVm.FirstOrDefault();
                foreach (var key in DimensionList)
                {
                    itemObj[key] = PropertyInfoDict[key].GetValue(vm2);
                }
                foreach (var column in AllColumnList)
                {
                    vm2 = listVm.FirstOrDefault(dictFunc[column]);
                    if (vm2 != null)
                    {
                        foreach (string name in AllNumberField)
                        {
                            itemObj[name + column] = PropertyInfoDict[name].GetValue(vm2);
                        }
                    }
                }
                listResult.Add(itemObj);
            }
            return listResult;
        }

    }
}
