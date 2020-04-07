using Sandwych.SmartConfig.Protocol;

namespace Sandwych.SmartConfig
{
    public interface ISmartConfigProvider : ISmartConfigContextFactory
    {
        string Name { get; }

        IProcedureEncoder CreateProcedureEncoder();

        IDevicePacketInterpreter CreateDevicePacketInterpreter();
    }
}
