﻿
namespace RM.Resources.WindowsService
{
    using Configurators;
    using Interfaces;
    using System;

    public class HostConfigurator<SERVICE> where SERVICE : IMicroService
    {
        HostConfiguration<SERVICE> innerConfig;
        public HostConfigurator(HostConfiguration<SERVICE> innerConfig)
        {
            this.innerConfig = innerConfig;
        }

        public void SetName(string serviceName, bool force = false)
        {
            if (!string.IsNullOrEmpty(innerConfig.Name) || force)
                innerConfig.Name = serviceName;
        }

        public void SetDisplayName(string displayName, bool force = false)
        {
            if (!string.IsNullOrEmpty(innerConfig.DisplayName) || force)
                innerConfig.DisplayName = displayName;
        }

        public void SetDescription(string description, bool force = false)
        {
            if (!string.IsNullOrEmpty(innerConfig.Description) || force)
                innerConfig.Description = description;
        }

        public string GetDefaultName() => innerConfig.Name;

        public bool IsNameNullOrEmpty
        {
            get
            {
                return string.IsNullOrEmpty(innerConfig.Name);
            }
        }

        public bool IsDescriptionNullOrEmpty
        {
            get
            {
                return string.IsNullOrEmpty(innerConfig.Description);
            }
        }

        public bool IsDisplayNameNullOrEmpty
        {
            get
            {
                return string.IsNullOrEmpty(innerConfig.DisplayName);
            }
        }

        public void Service(Action<ServiceConfigurator<SERVICE>> serviceConfigAction)
        {
            try
            {
                var serviceConfig = new ServiceConfigurator<SERVICE>(innerConfig);
                serviceConfigAction(serviceConfig);
                if (innerConfig.ServiceFactory == null)
                    throw new ArgumentException("It's necesarry to configure action that creates the service (ServiceFactory)");
                
                if (innerConfig.OnServiceStart == null)
                    throw new ArgumentException("It's necesarry to configure action that is called when the service starts");
                
            }
            catch (Exception e)
            {
                throw new ArgumentException("Configuring the service thrown an exception", e);
            }
        }
    }
}
