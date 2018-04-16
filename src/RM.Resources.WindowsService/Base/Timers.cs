
namespace RM.Resources.WindowsService.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Timers
    {
        List<Timer> timers = new List<Timer>();
               
        public void Start(string timerName, int interval, Action onTimer, Action<Exception> onException = null)
        {
            var tmpTimer = GetTimerBy(timerName);

            if (tmpTimer == null)
            {
                tmpTimer = new Timer(timerName, interval, onTimer, onException);
                timers.Add(tmpTimer);

                tmpTimer.Start();
            }
            else
            {
                tmpTimer.Stop();
                Update(timerName, interval, onTimer, onException);
                tmpTimer.Start();
            }
        }

        public void Update(string timerName, int interval = 0, Action onTimer = null, Action<Exception> onException = null)
        {
            var tmpTimer = GetTimerBy(timerName);

            ExecuteActionIfTimerIsNotNull(tmpTimer, () =>
            {
                tmpTimer
                .ConfigOnTimer(onTimer)
                .ConfigOnException(onException)
                .ConfigInterval(interval);
            });
        }

        public void Resume() => timers?.ForEach(timer => {  timer.Resume(); });        

        public void Resume(string timerName)
        {
            var tmpTimer = GetTimerBy(timerName);
            ExecuteActionIfTimerIsNotNull(tmpTimer, () => { tmpTimer.Resume(); });
        }

        public void Pause() => timers?.ForEach(timer => { timer.Pause(); });

        public void Pause(string timerName)
        {
            var tmpTimer = GetTimerBy(timerName);
            ExecuteActionIfTimerIsNotNull(tmpTimer, () => { tmpTimer.Pause(); });
        }

        public void Stop() => timers?.ForEach(timer => { timer.Stop(); });

        public void Stop(string timerName)
        {
            var tmpTimer = GetTimerBy(timerName);
            ExecuteActionIfTimerIsNotNull(tmpTimer, () => { tmpTimer.Stop(); });            
        }

        private Timer GetTimerBy(string timerName) => timers
                .Where(x => x.Name == timerName)
                .FirstOrDefault();

        private void ExecuteActionIfTimerIsNotNull(Timer timer, Action action)
        {
            if (timer == null)
                return;

            action();           
        }
    }
}
