using System;

namespace DotNetCore.Common
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
        public static AppConfigurations AppConfigurations { get; set; }
    }
}
