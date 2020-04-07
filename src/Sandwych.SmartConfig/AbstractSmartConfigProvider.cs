using Sandwych.SmartConfig.Protocol;
using System.Collections.Generic;

namespace Sandwych.SmartConfig
{
    public abstract class AbstractSmartConfigProvider : ISmartConfigProvider
    {
        public abstract string Name { get; }

        public abstract IDevicePacketInterpreter CreateDevicePacketInterpreter();

        public SmartConfigContext CreateContext()
        {
            var ctx = new SmartConfigContext(this);
            foreach(var e in this.GetDefaultOptions())
            {
                ctx.Options[e.key] = e.value;
            }
            return ctx;
        }

        public abstract IProcedureEncoder CreateProcedureEncoder();

        public abstract IEnumerable<(string key, object value)> GetDefaultOptions();
    }
}
