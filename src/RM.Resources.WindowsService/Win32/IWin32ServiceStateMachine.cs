
namespace RM.Resources.WindowsService.Win32
{
    using Annotations;

    [PublicAPI]
    public interface IWin32ServiceStateMachine
    {
        void OnStart(string[] startupArguments, ServiceStatusReportCallback statusReportCallback);

        void OnCommand(ServiceControlCommand command, uint commandSpecificEventType);
    }
}
