
namespace RM.Resources.Schedules
{
    using System;
    using System.Threading;

    public class JobContext
    {

        public Action<Job> CallbackAction { get; protected set; }

        public Job ManagedJob { get; set; }

        public DateTimeOffset? LastJobEvaluation { get; set; }

        public DateTimeOffset? NextExecution { get; set; }

        public int? RemainingExecutions { get; set; }

        public JobContext(Job managedJob, Action<Job> callbackAction)
        {
            if (managedJob == null) throw new ArgumentNullException("managedJob");
            TimeSpan? interval = managedJob.Interval;
            if (interval == null && (managedJob.Loops == null || managedJob.Loops.Value > 1))
            {                
                string msg = "Job [{0}] inválido: especifique um único ou um intervalo para executar.";
                msg = String.Format(msg, managedJob.JobId);
                throw new InvalidOperationException(msg);
            }

            ManagedJob = managedJob;
            CallbackAction = callbackAction ?? throw new ArgumentNullException("callbackAction");
            NextExecution = managedJob.StartTime;

            var now = SystemTime.Now();
            if (NextExecution.Value < now) NextExecution = now;

            RemainingExecutions = managedJob.Loops;
        }

        public virtual void ExecuteAsync(Scheduler scheduler)
        {
            if (ManagedJob.State == JobState.Active && (ManagedJob.ExpirationTime == null || ManagedJob.ExpirationTime >= SystemTime.Now()))
            {
                ThreadPool.QueueUserWorkItem(s =>
                {
                    try
                    {
                        CallbackAction(ManagedJob);
                    }
                    catch (Exception e)
                    {
                        Scheduler sch = (Scheduler)s;
                        if (!sch.SubmitJobException(ManagedJob, e)) throw e;
                    }
                }, scheduler);
            }

            UpdateState();
        }

        protected virtual void UpdateState()
        {
            LastJobEvaluation = SystemTime.Now();

            lock (ManagedJob.SyncRoot)
            {
                if (ManagedJob.State == JobState.Canceled)
                {
                    NextExecution = null;
                    return;
                }

                if (RemainingExecutions.HasValue && ManagedJob.State == JobState.Active)
                {
                    RemainingExecutions--;
                }
                else if (RemainingExecutions.HasValue && RemainingExecutions == 0)
                {
                    ManagedJob.State = JobState.Finished;
                    NextExecution = null;
                    return;
                }                

                if (!ManagedJob.Interval.HasValue)
                {
                    ManagedJob.State = JobState.Canceled;
                    NextExecution = null;
                    return;
                }

                NextExecution = (NextExecution.Value).Add(ManagedJob.Interval.Value);

                if (ManagedJob.ExpirationTime.HasValue && LastJobEvaluation.Value > ManagedJob.ExpirationTime)
                {
                    ManagedJob.State = JobState.Finished;
                    NextExecution = null;
                }
            }
        }

    }
}
