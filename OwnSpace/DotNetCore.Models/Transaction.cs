using DotNetCore.Models.Basic;
using DotNetCore.Models.Enums;
using System.ComponentModel.DataAnnotations;


namespace DotNetCore.Models
{
	public class Transaction : EntityBase<int>
	{
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[Display(Name = "名称")]
		public int Name { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2048)]
		[Display(Name = "备注")]
		public string Remarks { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		[Required]
		[Display(Name = "状态")]
		public TransactionStatus TransactionStatus { get; set; }
	}
}
