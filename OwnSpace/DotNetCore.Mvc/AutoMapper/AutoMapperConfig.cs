using AutoMapper;
using DotNetCore.Models;
using System;

namespace DotNetCore.Mvc.AutoMapper
{
	using Extensions;
	using Models;

	public class AutoMapperConfig
	{
		/// <summary>
		/// 是否已经进行过配置
		/// </summary>
		private static volatile bool _hasInitialized;

		/// <summary>
		/// 无值时占位符
		/// </summary>
		public const string None = @"--";

		public static void CreateMappings()
		{
			if (_hasInitialized)
			{
				return;
			}

			CreateMappingsInternal();
			_hasInitialized = true;
		}

		private static void CreateMappingsInternal()
		{
			Mapper.Initialize(config =>
			{
				#region Basic Types

				// 为AutoMapper配置基本数据类型之间的Map方式。
				// 涉及到具体类型的成员Map时，如有其他需要（如指定decimal的特殊精度），
				// 可在该类型的CreateMap()中使用ForMember()/ResolveUsing()等方法覆盖本段配置中提供的转换方法。

				config.CreateMap<int?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString() : None);
				config.CreateMap<decimal, string>().ConvertUsing(src => src.ToString("N2"));
				config.CreateMap<decimal?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString("N2") : None);
				config.CreateMap<double?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString("N2") : None);
				config.CreateMap<DateTime, string>().ConvertUsing(src => src.ToString("yyyy-MM-dd"));
				config.CreateMap<DateTime?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString("yyyy-MM-dd") : None);
				config.CreateMap<Enum, string>().ConvertUsing(src => src.GetDescription() ?? src.GetDisplayName() ?? src.ToString());

				#endregion

				#region BoudTransactionDetail => BoudTransactionDetailViewModel

				config.CreateMap<User, UserViewModel>()
				.ForMember(d => d.Birthday, opt => opt.MapFrom(src => src.Birthday.ToString("yyyy-MM-dd HH:mm")));

				#endregion
			});
		}
	}
}
