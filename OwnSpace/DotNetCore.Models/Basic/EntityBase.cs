using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Models.Basic
{
	public class EntityBase<T>
	{
		[Key]
		public T ID { get; set; }

		public DateTime TimeStamp { get; set; } = DateTime.Now;

		public DateTime CreatedTime { get; set; } = DateTime.Now;

		public DeleteState DeleteState { get; set; } = DeleteState.Valid;

		[Timestamp]
		public byte[] RowVersion { get; set; }
	}

	public enum DeleteState
	{
		Deleted = 0,
		Valid = 1
	}
}
