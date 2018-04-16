

namespace RM.Resources.WindowsService
{
    using Interfaces;
    using System;

    public class MicroServiceController : IMicroServiceController
    {
        private Action stop;

        public MicroServiceController(Action stop)
        {
            this.stop = stop;
        }

        public void Stop()
        {
            stop();
        }
    }
}
