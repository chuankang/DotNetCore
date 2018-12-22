using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Models
{
    public class UserViewModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

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
