using System;
using System.ComponentModel.DataAnnotations;
using DotNetCore.Models.Basic;
using DotNetCore.Models.Enums;

namespace DotNetCore.Models
{
    public class BasicInfomation : EntityBase<int>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        [Display(Name = "会员等级")]
        public MemberShipLevel MemberLevel { get; set; }
    }
}
