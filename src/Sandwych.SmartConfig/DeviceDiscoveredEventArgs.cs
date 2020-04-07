using System;

namespace Sandwych.SmartConfig
{
    public class DeviceDiscoveredEventArgs : EventArgs
    {
        public DeviceDiscoveredEventArgs(ISmartConfigDevice device)
        {
            this.Device = device;
        }

        public ISmartConfigDevice Device { get; }
    }
}
