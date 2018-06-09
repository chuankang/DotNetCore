using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Mvc.Models
{
    public class UserViewModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 出生年月日
        /// </summary>
        public string Birthday { get; set; }
    }
}
