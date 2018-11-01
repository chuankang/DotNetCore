using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OrchardCore.Environment.Navigation;

namespace SaaS.Services
{
    public class AdminMenu:INavigationManager
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public IEnumerable<MenuItem> BuildMenu(string name, ActionContext context)
        {
            throw new NotImplementedException();
        }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder.Add(T["WeChat"], configuration => configuration
                .Add(T["Setting"], "1", setting => setting
                    .Action("Index", "Admin", new
                    {
                        area = "WeChat"
                    })
                    .Permission(Permissions.WechatSettings)
                    .LocalNav()
                ));
        }

    }
}
