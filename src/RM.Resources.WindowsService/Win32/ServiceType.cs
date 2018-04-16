
namespace RM.Resources.WindowsService.Win32
{
    using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS1701 // Assuming assembly reference matches identity
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "External API")]
#pragma warning restore CS1701 // Assuming assembly reference matches identity
    internal enum ServiceType : uint
    {
        FileSystemDriver = 0x00000002,
        KernelDriver = 0x00000001,
        Win32OwnProcess = 0x00000010,
        Win32ShareProcess = 0x00000020,
        InteractiveProcess = 0x00000100
    }
}
