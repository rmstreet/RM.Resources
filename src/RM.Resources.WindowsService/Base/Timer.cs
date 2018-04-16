
namespace RM.Resources.WindowsService.Base
{
    using System;
    using System.Threading;

    public class Timer
    {
        private Thread thread;
        private AutoResetEvent stopRequest;
        private bool running = true;
        private bool paused = false;

        public Timer(string name, int interval, Action onTimer, Action<Exception> onException = null)
        {
            this.OnTimer = onTimer ?? (() => { });
            this.Name = name;
            this.Interval = interval;
            this.OnException = onException ?? ((e) => { });
        }

        public Action OnTimer { get; private set; }

        public Action<Exception> OnException { get; private set; }

        public string Name { get; private set; }

        public int Interval { get; private set; }


        public Timer ConfigOnTimer(Action action)
        {
            if (action == null)
                return this;

            this.OnTimer = action;
            return this;
        }

        public Timer ConfigOnException(Action<Exception> onException)
        {
            if (onException == null)
                return this;

            this.OnException = onException;
            return this;
        }

        public Timer ConfigInterval(int interval)
        {
            if (interval <= 0 || interval == this.Interval)
                return this;

            this.Interval = interval;
            return this;
        }
               

        public void Start()
        {
            stopRequest = new AutoResetEvent(false);
            running = true;
            thread = new Thread(InternalWork);
            thread.Start();
        }

        public void Pause() => paused = true;

        public void Resume() => paused = false;

        public void Stop()
        {
            if (!running)
                return;
            
            running = false;
            stopRequest.Set();
            thread.Join();

            thread = null;
            stopRequest.Dispose();
            stopRequest = null;            
        }

        private void InternalWork(object arg)
        {
            while (running)
            {
                try
                {
                    if (!paused)
                    {
                        this.OnTimer();
                    }
                }
                catch (Exception exception)
                {
                    this.OnException(exception);
                }

                try
                {
                    if (stopRequest.WaitOne(Interval))
                    {
                        return;
                    }
                }
                catch (Exception exception)
                {
                    this.OnException(exception);
                }

            }
        }

    }
}
