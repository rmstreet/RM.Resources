
namespace RM.Resources.WindowsService.Win32
{
    public delegate void ServiceStatusReportCallback(ServiceState state, ServiceAcceptedControlCommandsFlags acceptedControlCommands, int win32ExitCode, uint waitHint);
}
