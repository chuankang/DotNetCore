using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.Security.Permissions;

namespace SaaS.Services
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission WechatSettings = new Permission("WeChatSettings", "WeChat settings");

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name="Administrator",//"Administrator",Authenticated
                    Permissions = new[]{WechatSettings}
                }
            };
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { WechatSettings };
        }
    }
}
