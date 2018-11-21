using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetCore.Common
{
    public static class CommonConvertor
    {
        /// <summary>
        /// 判断是否是数字
        /// </summary>
        public static bool IsNumber(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return false;

            return double.TryParse(obj.ToString(), out double _);
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        public static double ToDouble(this object obj)
        {
            if (obj.IsNumber())
            {
                return Convert.ToDouble(obj);
            }

            return 0;
        }

        /// <summary>
        /// 转为decimal
        /// </summary>
        public static decimal ToDecimal(this object obj)
        {
            return obj.IsNumber() ? Convert.ToDecimal(obj) : 0m;
        }

        /// <summary>
        /// 转为decimal，并指定保留几位小数
        /// </summary>
        public static decimal? ToDecimal(this object obj, int length)
        {
            if (obj == null || !obj.IsNumber())
                return null;
            return Math.Round(Convert.ToDecimal(obj), length, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 转为bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object obj)
        {
            if (obj == null)
                throw new NullReferenceException();

            if (bool.TryParse(obj.ToString(), out bool b))
                return b;
            else switch (obj.ToString())
                {
                    case "1":
                        return true;
                    case "0":
                        return false;
                    default:
                        throw new ArgumentException("Coudn't cast");
                }
        }

        /// <summary>
        /// int转string
        /// </summary>
        public static string ToString(this int value, int length)
        {
            string valStr = value.ToString();

            if (valStr.Length > length)
                return valStr;

            var builder = new StringBuilder(valStr);
            int count = length - valStr.Length;
            while (count > 0)
            {
                builder.Insert(0, '0');
                count--;
            }

            return builder.ToString();
        }

        /// <summary>
        /// 转DateTime
        /// </summary>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null)
                throw new NullReferenceException();

            if (DateTime.TryParse(obj.ToString(), out var d))
                return d;
            return DateTime.TryParseExact(obj.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out d)
                ? d : default(DateTime);
        }
        
        public static T ToEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static T? TryToEnum<T>(this string str) where T : struct
        {
            if (Enum.TryParse<T>(str, out var t))
                return t;
            return null;
        }

        public static string ToEnumString(this Enum en)
        {
            Type temType = en.GetType();
            MemberInfo[] memberInfos = temType.GetMember(en.ToString());
            if (memberInfos != null && memberInfos.Length > 0)
            {
                object[] objs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    return ((DescriptionAttribute)objs[0]).Description;
                }
            }

            return en.ToString();
        }
        
        /// <summary>
        /// 取一天的结束时间
        /// </summary>
        public static DateTime ToDayEndTime(this DateTime time)
        {
            return time >= DateTime.MaxValue.AddDays(-1) 
                ? time : time.Date.AddDays(1).AddSeconds(-1);
        }

        public static DateTime ToDayCloseTime(this DateTime time)
        {
            if (time >= DateTime.MaxValue.AddDays(-1))
                return time;

            return time.Date.AddHours(15);
        }

        /// <summary>
        /// 将object转换为System.Int32, 失败时返回0
        /// </summary>
        public static int ToInt32(this object obj)
        {
            if (obj.IsNumber())
                return Convert.ToInt32(obj);

            if (obj is Enum)
                return (int)obj;

            return 0;
        }

        public static FieldInfo[] GetFieldInfoArray<T>()
        {
            FieldInfo[] props = null;
            try
            {
                Type type = typeof(T);
                object obj = Activator.CreateInstance(type);
                props = type.GetFields();
            }
            catch (Exception ex)
            { }
            return props;
        }

        /// <summary>
        /// 获取枚举的Description属性，找不到时返回其ToString()
        /// </summary>
        public static string GetDescriptionSafely<TEnum>(this TEnum value) where TEnum : struct
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .OfType<DescriptionAttribute>()
                .ToList();

            return attributes.Any() ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// 将decimal按输入的精度（默认为2）四舍五入后转换为带逗号分隔的字符串返回
        /// </summary>
        public static string ToShowingString(this decimal value, int precision = 2)
        {
            return Math.Round(value, precision, MidpointRounding.AwayFromZero).ToString("N" + precision);
        }

        /// <summary>
        /// 将DateTime对象格式化为字符串，默认为yyyy-MM-dd格式
        /// </summary>
        public static string ToDateString(this DateTime datetime, string format = "yyyy-MM-dd")
        {
            return datetime.ToString(format);
        }

        /// <summary>
        /// 将Int32对象转换为枚举
        /// </summary>
        public static TEnum ToEnum<TEnum>(this int value)
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new InvalidOperationException("无法使用未定义的值进行枚举转换。");

            return (TEnum)Enum.ToObject(typeof(TEnum), value);
        }

        /// <summary>
        /// 将字符串转为全拼
        /// </summary>
        public static string ToFullPinYin(this string str)
        {
            return CPinyinConverter.Get(str);
        }

        /// <summary>
        /// 将字符串转为简拼
        /// </summary>
        public static string ToFirstPinYin(this string str)
        {
            return CPinyinConverter.GetFirst(str);
        }

        /// <summary>
        /// 对decimal值做四舍五入处理
        /// </summary>
        /// <param name="value">待处理值</param>
        /// <param name="precision">转换精度</param>
        /// <returns>舍入后的值</returns>
        public static decimal RoundAwayFromZero(this decimal value, int precision = 2)
        {
            return Math.Round(value, precision, MidpointRounding.AwayFromZero);
        }
        

        public static dynamic ToExpandoObject(this object obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                object currentValue = propertyInfo.GetValue(obj);
                expando.Add(propertyInfo.Name, currentValue);
            }

            return (ExpandoObject)expando;
        }
    }
}
