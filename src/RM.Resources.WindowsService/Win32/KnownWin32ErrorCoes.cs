
namespace RM.Resources.WindowsService.Win32
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class KnownWin32ErrorCoes
    {
        internal const int ERROR_SERVICE_ALREADY_RUNNING = 1056;
        internal const int ERROR_SERVICE_DOES_NOT_EXIST = 1060;
    }
}
