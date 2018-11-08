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

    public enum MemberShipLevel
    {
        非会员 = 0,
        青铜 = 1,
        白银 = 2,
        黄金 = 3,
        铂金 = 4,
        钻石 = 5
    }
}
