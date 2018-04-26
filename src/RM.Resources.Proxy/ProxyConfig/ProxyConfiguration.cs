using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RM.Resources.Proxy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RM.Resources.Proxy.ServicesConfig.ApiConfig;

namespace RM.Resources.Proxy.ProxyConfig
{
    public class ProxyConfiguration
    {
        public ProxyOptions GetOptions(HttpContext context)
        {
            Uri currentUri = GetCurrentUri(context);

            var (scheme, host, port) = GetUriComponents(currentUri);

            return new ProxyOptions
            {
                Scheme = scheme,
                Host = host,
                Port = port,
            };
        }

        public ProxyOptions GetOptions(EBasePaths path)
        {
            InMemoryServicePaths inMemoryServices = InMemoryServicePaths.Instance();
            var service = inMemoryServices.GetService(path);
            var (scheme, host, port) = GetUriComponents(service.Config.UriBase);
            return new ProxyOptions()
            {
                Scheme = scheme,
                Host = host,
                Port = port
            };

        }

        private Uri GetCurrentUri(HttpContext context)
        {
            InMemoryServicePaths memory = InMemoryServicePaths.Instance();

            List<ServiceInitialPath> services = memory.GetServices(context.Request.Path);

            ServiceInitialPath service = services?.FirstOrDefault();
            if (service is null)
                return null;
            return service.Config.UriBase;
        }

        private (string scheme, string host, string port) GetUriComponents(Uri uri)
        {
            string scheme = uri.GetComponents(UriComponents.Scheme, UriFormat.Unescaped);

            string host = uri.GetComponents(UriComponents.Host, UriFormat.Unescaped);

            string port = uri.GetComponents(UriComponents.Port, UriFormat.Unescaped);

            return (scheme, host, port);
        }
    }
}
