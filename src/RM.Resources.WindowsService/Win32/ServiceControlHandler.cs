
namespace RM.Resources.WindowsService.Win32
{
    using System;

    internal delegate void ServiceControlHandler(ServiceControlCommand control, uint eventType, IntPtr eventData, IntPtr eventContext);
}
