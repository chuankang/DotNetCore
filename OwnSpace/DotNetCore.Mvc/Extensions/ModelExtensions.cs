using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNetCore.Mvc.Extensions
{
	public static class ModelExtensions
	{
		private static List<T> GetFieldAttributes<T>(this object value) where T : Attribute
		{
			FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
			var attributes = fieldInfo?.GetCustomAttributes(typeof(T), false) ?? Enumerable.Empty<object>();

			return attributes.OfType<T>().ToList();
		}

		private static List<T> GetPropAttributes<T>(this PropertyInfo value) where T : Attribute
		{
			var attributes = value.GetCustomAttributes(typeof(T), false) ?? Enumerable.Empty<object>();
			return attributes.OfType<T>().ToList();
		}

		/// <summary>
		/// 获取对象的Display特性的Name属性，找不到时返回null
		/// </summary>
		public static string GetDisplayName<T>(this T value)
		{
			var attributes = value.GetFieldAttributes<DisplayAttribute>();
			return attributes.FirstOrDefault()?.Name;
		}

		/// <summary>
		/// 获取对象的Display特性的Name属性，找不到时返回其ToString()
		/// </summary>
		public static string GetDisplayNameSafely<T>(this T value)
		{
			var attributes = value.GetFieldAttributes<DisplayAttribute>();
			return attributes.FirstOrDefault()?.Name ?? value.ToString();
		}

		/// <summary>
		/// 获取枚举的Description特性，找不到时返回null
		/// </summary>
		public static string GetDescription(this Enum value)
		{
			var attributes = value.GetFieldAttributes<DescriptionAttribute>();
			return attributes.FirstOrDefault()?.Description;
		}

		/// <summary>
		/// 获取枚举的Description特性，找不到时返回其ToString()
		/// </summary>
		public static string GetDescriptionSafely(this Enum value)
		{
			var attributes = value.GetFieldAttributes<DescriptionAttribute>();
			return attributes.FirstOrDefault()?.Description ?? value.ToString();
		}

		/// <summary>
		/// 根据属性名得到属性的描述
		/// </summary>
		public static string GetDescriptionByProperty(this object obj, string propName)
		{
			PropertyInfo prop = obj.GetType().GetProperty(propName);

			string description =
				prop.GetPropAttributes<DescriptionAttribute>().FirstOrDefault()?.Description ??
				prop.GetPropAttributes<DisplayAttribute>().FirstOrDefault()?.Name ??
				prop.GetPropAttributes<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ??
				prop.ToString();

			return description;
		}
	}
}

