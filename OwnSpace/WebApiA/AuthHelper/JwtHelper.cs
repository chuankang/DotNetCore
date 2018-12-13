using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiA.Models;

namespace WebApiA.AuthHelper
{
    /// <summary>
    /// JWT序列化和反序列化
    /// </summary>
    public class JwtHelper
    {
        public static string SecretKey { get; set; } = "sdfsdfsrty45634kkhllghtdgdfss345t678fs";
        
        /// <summary>
        /// 颁发JWT字符串 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenJwt token)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Jti,token.Uid.ToString()),//Id
                new Claim("Role", token.Role),//角色
                new Claim(JwtRegisteredClaimNames.Iat,dateTime.ToString(CultureInfo.InvariantCulture),ClaimValueTypes.Integer64)
            };
            //秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtHelper.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "Blog.Core",
                claims: claims, //声明集合
                expires: dateTime.AddHours(2),
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            string encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        public static TokenJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue("Role", out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenJwt
            {
                Uid = Convert.ToInt32(jwtToken.Id),
                Role = role != null ? role.ToString() : "",
            };

            return tm;
        }
    }
}
