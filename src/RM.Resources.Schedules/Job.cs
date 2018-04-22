
namespace RM.Resources.Schedules
{
    using System;

    public class Job
    {

        public string JobId { get; set; }

        public object SyncRoot
        {
            get { return this; }
        }

        public virtual DateTimeOffset StartTime { get; set; }

        private TimeSpan? interval;

        public virtual TimeSpan? Interval
        {
            get { return interval; }
            set
            {
                if (interval.HasValue && interval.Value.TotalMilliseconds < 0)
                {
                    string msg = "O intervalo de {0} milissegundos é inválido. O intervalo deve ter um valor positivo ou nulo [null].";
                    msg = String.Format(msg, interval.Value.TotalMilliseconds);
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                lock (SyncRoot)
                {
                    interval = value;
                }
            }
        }

        private int? loops;

        public virtual int? Loops
        {
            get { return loops; }
            set
            {
                if (value.HasValue && value < 1)
                {
                    string msg = "Número inválido de loops: {0}. Apenas números acima de zero ou nulos [null] são permitidos.";
                    msg = String.Format(msg, value);
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                lock (SyncRoot)
                {
                    loops = value;
                }
            }
        }

        private DateTimeOffset? expirationTime;

        public virtual DateTimeOffset? ExpirationTime
        {
            get { return expirationTime; }
            set
            {
                if (value.HasValue && value < SystemTime.Now())
                {
                    const string msg = "A expiração não pode ser no passado.";
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                lock (SyncRoot)
                {
                    expirationTime = value;
                }
            }
        }

        public virtual JobState State { get; protected internal set; }

        public Job()
            : this(Guid.NewGuid().ToString())
        {
        }

        public Job(string jobId)
        {
            JobId = jobId;
        }

        public virtual JobSchedule Run
        {
            get { return new JobSchedule(this); }
        }

        public virtual void Cancel()
        {
            lock (this)
            {
                State = JobState.Canceled;
            }
        }

        public virtual bool Pause()
        {
            if (!Interval.HasValue)
            {
                const string msg = "Jobs sem intervalo no pode ser pausado. "
                                   + "- O scheduler não sabe como proceder quando retornado sem intervalo.";
                throw new InvalidOperationException(msg);
            }

            lock (this)
            {
                if (State != JobState.Active) return false;
                State = JobState.Paused;
                return true;
            }
        }

        public virtual bool Resume()
        {
            lock (this)
            {
                if (State != JobState.Paused) return false;
                State = JobState.Active;
                return true;
            }
        }
    }

    public class Job<T> : Job
    {
        public T Data { get; set; }

        public Job()
        {
        }

        public Job(string id)
            : base(id)
        {
        }
    }
}
