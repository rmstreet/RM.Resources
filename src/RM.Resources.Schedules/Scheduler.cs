
namespace RM.Resources.Schedules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Scheduler : IDisposable
    {

        protected long LastTimerInterval { get; set; }

        protected DateTimeOffset LastTimerRun { get; private set; }

        protected bool IsDisposed { get; set; }

        public ReschedulingStrategy SystemTimeChangeRescheduling { get; set; }

        public Action<Job, Exception> JobExceptionHandler { get; set; }

        private int selfTestInterval = 120000;

        public int SelfTestInterval
        {
            get { return selfTestInterval; }
            set
            {
                if (value < 100)
                {
                    const string msg = "O intervalo deve ser maior do que 100 milissegundos.";
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                selfTestInterval = value;

                lock (SyncRoot)
                {
                    Reschedule();
                }

            }
        }

        private int minJobInterval = 100;

        public int MinJobInterval
        {
            get { return minJobInterval; }
            set
            {
                if (value < 1)
                {
                    const string msg = "O Intervalo deve ser maior do que zero milissegundos.";
                    throw new ArgumentOutOfRangeException("value", msg);
                }
                minJobInterval = value;
            }
        }

        protected Timer Timer { get; set; }

        protected List<JobContext> Jobs { get; set; }

        private readonly object syncRoot = new object();

        public object SyncRoot
        {
            get { return syncRoot; }
        }

        protected bool IsSorted { get; set; }

        public DateTimeOffset? NextExecution { get; protected set; }

        public Scheduler()
        {
            Jobs = new List<JobContext>();
            Timer = new Timer(ProcessJobs, null, Timeout.Infinite, Timeout.Infinite);
        }

        public virtual void SubmitJob<T>(Job<T> job, Action<Job<T>, T> callback)
        {
            Action<Job> action = j =>
            {
                Job<T> genericJob = (Job<T>)j;
                callback(genericJob, genericJob.Data);
            };

            SubmitJob(job, action);
        }

        public virtual void SubmitJob(Job job, Action<Job> callback)
        {
            if (job == null) throw new ArgumentNullException("job");
            if (callback == null) throw new ArgumentNullException("callback");

            TimeSpan? interval = job.Interval;
            if (interval.HasValue && interval.Value.TotalMilliseconds < MinJobInterval)
            {
                string msg = "Intervalo de {0} ms é muito pequeno - intervalo mínimo de {1} ms";
                msg = String.Format(msg, interval.Value.TotalMilliseconds, MinJobInterval);
                throw new InvalidOperationException(msg);
            }

            JobContext context = new JobContext(job, callback);

            lock (SyncRoot)
            {
                if (NextExecution == null || context.NextExecution <= NextExecution.Value)
                {
                    Jobs.Insert(0, context);

                    if (NextExecution == null || NextExecution.Value.Subtract(SystemTime.Now()).TotalMilliseconds > MinJobInterval)
                        Reschedule();
                }
                else
                {
                    Jobs.Add(context);
                    IsSorted = false;
                }
            }
        }

        public virtual Job TryGetJob(string jobId)
        {
            lock (SyncRoot)
            {
                var context = Jobs.FirstOrDefault(jc => jc.ManagedJob.JobId == jobId);
                return context == null ? null : context.ManagedJob;
            }
        }

        public virtual bool PauseJob(string jobId)
        {
            var job = TryGetJob(jobId);
            return job == null ? false : job.Pause();
        }

        public virtual bool ResumeJob(string jobId)
        {
            var job = TryGetJob(jobId);
            return job == null ? false : job.Resume();
        }

        public virtual bool CancelJob(string jobId)
        {
            lock (SyncRoot)
            {
                for (int i = 0; i < Jobs.Count; i++)
                {
                    var job = Jobs[i];
                    if (job.ManagedJob.JobId == jobId)
                    {
                        Jobs.RemoveAt(i);
                        job.ManagedJob.Cancel();

                        if (i == 0) Reschedule();

                        return true;
                    }
                }
            }

            return false;
        }

        public virtual void CancelAll()
        {
            lock (SyncRoot)
            {
                if (IsDisposed) return;

                Timer.Change(Timeout.Infinite, Timeout.Infinite);
                NextExecution = null;
                Jobs.Clear();
            }
        }

        protected virtual void ProcessJobs(object state)
        {
            lock (SyncRoot)
            {
                if (IsDisposed) return;

                if (SystemTimeChangeRescheduling != ReschedulingStrategy.KeepFixedTimes)
                    VerifySystemTime();

                RunPendingJobs();

                if (!IsSorted) SortJobs();

                Reschedule();
            }
        }

        protected virtual void VerifySystemTime()
        {

            if (LastTimerInterval == Timeout.Infinite) return;

            var now = SystemTime.Now();
            var pauseDuration = now.Subtract(LastTimerRun);
            var delta = pauseDuration.TotalMilliseconds - LastTimerInterval;

            if (delta > 1000 || delta < 1000)
            {

                bool changeExpirationTime = SystemTimeChangeRescheduling ==
                                            ReschedulingStrategy.RescheduleNextExecutionAndExpirationTime;
                Jobs.ForEach(jc =>
                {
                    jc.NextExecution = jc.NextExecution.Value.AddMilliseconds(delta);
                    if (changeExpirationTime && jc.ManagedJob.ExpirationTime.HasValue)
                        jc.ManagedJob.ExpirationTime = jc.ManagedJob.ExpirationTime.Value.AddMilliseconds(delta);
                });
            }

        }

        protected virtual void SortJobs()
        {
            Jobs.Sort((first, second) => first.NextExecution.Value.CompareTo(second.NextExecution.Value));
            IsSorted = true;
        }

        protected virtual void RunPendingJobs()
        {

            DateTimeOffset now = SystemTime.Now();
            List<JobContext> dueJobs = new List<JobContext>();
            
            for (int i = 0; i < Jobs.Count; i++)
            {
                var job = Jobs[i];

                if (job.NextExecution.Value > now) break;

                dueJobs.Add(job);
            }

            JobContext currentJob = null;
            try
            {                
                for (int i = dueJobs.Count - 1; i >= 0; i--)
                {
                    currentJob = dueJobs[i];

                    currentJob.ExecuteAsync(this);

                    if (currentJob.NextExecution.HasValue)
                        IsSorted = false;
                    else
                        Jobs.RemoveAt(i);
                }
            }
            catch (Exception e)
            {
                if (!SubmitJobException(currentJob.ManagedJob, e)) throw e;
            }
        }

        protected virtual void Reschedule()
        {
            if (IsDisposed) return;

            if (Jobs.Count == 0)
            {
                //desativar o temporizador se não tivermos trabalhos pendentes
                NextExecution = null;
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
                LastTimerInterval = Timeout.Infinite;
            }
            else
            {
                //agendar próximo evento
                var executionTime = Jobs[0].NextExecution;

                DateTimeOffset now = SystemTime.Now();
                TimeSpan delay = executionTime.Value.Subtract(now);

                //caso a próxima execução já estiver pendente, adicionar um atraso de segura
                int dueTime = (int)Math.Max(MinJobInterval, (long)delay.TotalMilliseconds);

                //alterar o temporizador - executar pelo menos, com o intervalo de auto-teste
                dueTime = Math.Min(dueTime, SelfTestInterval);

                NextExecution = SystemTime.Now().AddMilliseconds(dueTime);
                Timer.Change(dueTime, Timeout.Infinite);
                LastTimerInterval = dueTime;
            }

            LastTimerRun = SystemTime.Now();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public virtual void Dispose()
        {
            lock (SyncRoot)
            {
                if (IsDisposed) return;

                IsDisposed = true;
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
                Timer.Dispose();
            }
        }

        internal virtual bool SubmitJobException(Job job, Exception exception)
        {
            var handler = JobExceptionHandler;
            if (handler != null)
            {
                handler(job, exception);
                return true;
            }

            return false;
        }

    }
}
