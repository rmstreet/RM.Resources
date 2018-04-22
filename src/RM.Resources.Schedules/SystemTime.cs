
namespace RM.Resources.Schedules
{
    using System;

    public static class SystemTime
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static Func<DateTimeOffset> Now;

        public static void Reset()
        {
            Now = () => DateTimeOffset.Now;
        }

        static SystemTime()
        {
            Reset();
        }
    }
}
