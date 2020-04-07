using System;
using System.Collections.Generic;
using System.Text;

namespace Sandwych.SmartConfig
{
    public class SmartConfigTimerEventArgs : EventArgs
    {
        public TimeSpan Timeout { get; }
        public TimeSpan ExecutedTime { get; }
        public TimeSpan LeftTime => Timeout - ExecutedTime;

        public SmartConfigTimerEventArgs (TimeSpan timeout, TimeSpan elapsed)
        {
            this.Timeout = timeout;
            this.ExecutedTime = elapsed;
        }
    }
}
