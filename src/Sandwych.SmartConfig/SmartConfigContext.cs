using System;
using System.Collections.Generic;

namespace Sandwych.SmartConfig
{
    public class SmartConfigContext
    {
        public IDictionary<string, object> Options { get; } = new Dictionary<string, object>();

        public T GetOption<T>(string name) => (T)this.Options[name];

        public void SetOption<T>(string name, T value)
        {
            this.Options[name] = value;
        }

        public ISmartConfigProvider Provider { get; internal set; }

        public SmartConfigContext(ISmartConfigProvider provider)
        {
            this.Provider = provider;
        }

        public event EventHandler<DeviceDiscoveredEventArgs> DeviceDiscoveredEvent;

        public void ReportDevice(ISmartConfigDevice device)
        {
            this.DeviceDiscoveredEvent?.Invoke(this, new DeviceDiscoveredEventArgs(device));
        }
    }
}
