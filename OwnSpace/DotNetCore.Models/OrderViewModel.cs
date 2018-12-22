using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Models
{
    public class OrderViewModel
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// -3:用户拒收 -2:未付款的订单 -1：用户取消 0:待发货 1:配送中 2:用户确认收货
        /// </summary>
        public string  Status { get; set; }

        /// <summary>
        /// 0:货到付款 1:在线支付
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderSource { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public string TotalMoney { get; set; }
    }
}
