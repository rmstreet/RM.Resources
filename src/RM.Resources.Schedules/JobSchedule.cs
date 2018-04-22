
namespace RM.Resources.Schedules
{
    using System;

    public class JobSchedule
    {
        private readonly Job job;

        public JobSchedule(Job job)
        {
            this.job = job;
        }

        public JobSchedule From(DateTimeOffset startTime)
        {
            job.StartTime = startTime;
            return this;
        }

        public JobSchedule Once()
        {
            job.Loops = 1;
            job.ExpirationTime = null;
            job.Interval = null;
            return this;
        }

        internal JobSchedule EveryInternal(TimeSpan jobInterval)
        {
            job.Interval = jobInterval;
            return this;
        }

        public Intervals Every
        {
            get { return new Intervals(this); }
        }

        public JobSchedule Times(int loops)
        {
            job.Loops = loops;
            return this;
        }

        public JobSchedule Until(DateTimeOffset jobExpirationTime)
        {
            job.ExpirationTime = jobExpirationTime;
            return this;
        }
    }
}
