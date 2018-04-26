using Microsoft.Extensions.Configuration;
using RM.Resources.Proxy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Resources.Proxy.ServicesConfig
{
    public abstract class ApiConfig
    {

        protected readonly IConfiguration config;

        public abstract List<ServiceInitialPath> RegisteredControllers { get; }

        public ApiConfig(IConfiguration config)
        {
            this.config = config;
        }
        public abstract Uri UriBase { get; }
        public virtual string BasePathWithoutPortOrHost
        {
            get
            {
                Uri uri = UriBase;

                string scheme = uri.GetComponents(UriComponents.Scheme, UriFormat.Unescaped);

                string hostAndPort = uri.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped);

                string stringUri = uri.ToString();

                string finalUri = stringUri.Replace(scheme + ":/", "").Replace(hostAndPort + "/", "");
                while (finalUri.StartsWith("/"))
                {
                    finalUri = finalUri.Remove(0);
                }
                return finalUri;
            }
        }

        public enum EBasePaths
        {

        }
    }
}
