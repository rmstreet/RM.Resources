
namespace RM.Resources.WindowsService.Win32
{
    public delegate void ServiceStoppedCallback();

    public interface IWin32Service
    {
        string ServiceName { get; }

        void Start(string[] startupArguments, ServiceStoppedCallback serviceStoppedCallback);

        void Stop();
    }
}
