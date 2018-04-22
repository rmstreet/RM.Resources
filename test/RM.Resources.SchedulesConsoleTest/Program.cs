

namespace RM.Resources.SchedulesConsoleTest
{
    using Schedules;
    using System;

    class Program
    {
        private static Scheduler _scheduler = new Scheduler();

        static void Main(string[] args)
        {

            DateTime dt = DateTime.Now;

            Job openNotepadJob = new Job("JobOpenNotepadInstanceKey");
            openNotepadJob.Run.Every.Seconds(15);
            openNotepadJob.StartTime = dt;

            _scheduler.SubmitJob(openNotepadJob, BusinessJobs.JobOpenNotepadInstance);

            Console.WriteLine("Waiting...");
            Console.ReadKey();
        }
    }
}
