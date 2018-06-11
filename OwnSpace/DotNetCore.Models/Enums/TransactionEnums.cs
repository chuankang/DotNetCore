using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Models.Enums
{
	public enum TransactionStatus
	{
		已提交 = 0,
		已通过 = 100,
		未通过 = 200,
	}
}
