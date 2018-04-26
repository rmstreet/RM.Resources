using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RM.Resources.Proxy.ServicesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RM.Resources.Proxy.ServicesConfig.ApiConfig;

namespace RM.Resources.Proxy.Models
{
    public class InMemoryServicePaths
    {
        private static InMemoryServicePaths _inMemoryService;

        public static InMemoryServicePaths Instance()
        {
            if (_inMemoryService is null)
                _inMemoryService = new InMemoryServicePaths();
            return _inMemoryService;
        }
        public static void Configure(IApplicationBuilder builder, IConfiguration config)
        {
            _builder = builder;
            _config = config;
        }




        private static IConfiguration _config;
        private static IApplicationBuilder _builder;
        private InMemoryServicePaths()
        {
            if (_builder == null || _config == null)
                throw new InvalidOperationException("container not registered");
        }

        public ServiceInitialPath GetService(EBasePaths basePath, string currentPath = "")
        {
            return GetServices(currentPath)
                .Where(x => x.Path.Equals(basePath.ToString(), StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        public bool ServiceExist(EBasePaths basePath, string currentPath = "")
        {
            return GetService(basePath, currentPath) != null;
        }

        public List<ServiceInitialPath> GetServices(string requestPath = "")
        {
            List<ServiceInitialPath> services = new List<ServiceInitialPath>();

            foreach (var config in _builder.ApplicationServices.GetServices<ApiConfig>())
            {
                services.AddRange(config.RegisteredControllers);
            }
            if (!string.IsNullOrEmpty(requestPath))
                services = services.Where(x => x.IsPathOf(requestPath)).ToList();
            return services;
        }

    }
}
