﻿
namespace RM.Resources.WindowsService.Win32
{
    using Annotations;
    using System;
    using System.ComponentModel;

    [PublicAPI]
    public enum ServiceState : uint
    {
        Stopped = 0x00000001,
        StartPending = 0x00000002,
        StopPending = 0x00000003,
        Running = 0x00000004,
        ContinuePending = 0x00000005,
        PausePending = 0x00000006,
        Paused = 0x00000007,

#if NETSTANDARD2_0
        [Browsable(false)]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Misspelled, use '" + nameof(StartPending) + "' instead. This member will be removed in upcoming versions.", true)]
        StartPening = StartPending
    }
}
