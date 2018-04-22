
namespace RM.Resources.SchedulesConsoleTest
{
    using Schedules;
    using System.Diagnostics;
    public static class BusinessJobs
    {
        private static Scheduler _scheduler = new Scheduler();

        public static void JobOpenNotepadInstance(Job job)
        {
            Process.Start("notepad.exe");
        }

    }
}
