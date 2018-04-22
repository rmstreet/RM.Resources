
namespace RM.Resources.WindowsService.Annotations
{
    using System;

    [Flags]
    public enum ImplicitUseTargetFlags
    {
        Default = 1,
        Itself = 1,
        Members = 2,
        WithMembers = 3
    }
}
