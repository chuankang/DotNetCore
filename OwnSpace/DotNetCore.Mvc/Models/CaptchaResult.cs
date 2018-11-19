using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Mvc.Models
{
    /// <summary>
    /// 描述验证码信息类
    /// </summary>
    public class CaptchaResult
    {
        [Required]
        [StringLength(4)]
        public string CaptchaCode { get; set; }
        public byte[] CaptchaByteData { get; set; }
        public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
        public DateTime Timestamp { get; set; }
    }
}
