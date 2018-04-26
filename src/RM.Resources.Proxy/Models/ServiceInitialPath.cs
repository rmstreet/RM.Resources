using RM.Resources.Proxy.ServicesConfig;
using System;
using System.Collections.Generic;
using System.Text;
using static RM.Resources.Proxy.ServicesConfig.ApiConfig;

namespace RM.Resources.Proxy.Models
{
    public class ServiceInitialPath
    {
        public ServiceInitialPath(EBasePaths path, ApiConfig config)
        {
            Path = path.ToString();
            Config = config;
        }
        public string Path { get; private set; }
        public ApiConfig Config { get; private set; }
        public bool IsPathOf(string path)
        {
            bool isValidPath = path.StartsWith(@"/api/" + this.Path + "/", StringComparison.OrdinalIgnoreCase);

            return isValidPath;
        }

    }
}
