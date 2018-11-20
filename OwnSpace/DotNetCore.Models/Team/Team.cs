using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Models.Team
{
    public class Team
    {
        public int Id { get; set; }

        /// <summary>
        /// 球队所在地址
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Display(Name = "球队所在地址")]
        public string Address { get; set; }

        public DateTime CreatedTime { get; set; } 

        public int DeleteState { get; set; }

        /// <summary>
        /// 东部 西部
        /// </summary>
        [MaxLength(10)]
        [Display(Name = "东部西部")]
        public string Direction { get; set; }

        /// <summary>
        /// 球队名称
        /// </summary>
        [MaxLength(100)]
        [Display(Name = "球队名称")]
        public string Name { get; set; }

        /// <summary>
        /// 赛区
        /// </summary>
        [MaxLength(100)]
        [Display(Name = "赛区")]
        public string RaceArea { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public string Arena { get; set; }
    }
}
