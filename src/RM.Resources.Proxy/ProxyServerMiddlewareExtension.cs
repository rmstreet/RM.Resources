using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RM.Resources.Proxy.Models;
using RM.Resources.Proxy.ProxyConfig;
using RM.Resources.Proxy.ServicesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RM.Resources.Proxy.ServicesConfig.ApiConfig;

namespace RM.Resources.Proxy
{
    public static class ProxyServerMiddlewareExtension
    {
        public static IApplicationBuilder UseProxyServer(this IApplicationBuilder builder)
        {
            //TODO: resolver por injeção de dependência
            InMemoryServicePaths path = InMemoryServicePaths.Instance();

            var services = path.GetServices();

            foreach (var service in services)
            {

                builder.MapWhen((HttpContext context) => {
                    ApiConfig serviceConfig = service.Config;
                    bool routeIsOfService = serviceConfig.RegisteredControllers.Any(x => context.Request.Path.Value.ToLower().Contains(x.Path.ToLower()));
                    if (routeIsOfService)
                        context.Request.Path = serviceConfig.BasePathWithoutPortOrHost + context.Request.Path.Value;
                    return routeIsOfService;

                }, b => b.RunProxy(new ProxyConfiguration()
                .GetOptions((EBasePaths)Enum.Parse(typeof(EBasePaths), service.Path))
                ));
            }

            return builder;
        }
    }
}
