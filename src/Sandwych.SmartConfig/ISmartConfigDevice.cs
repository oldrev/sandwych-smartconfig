using System.Net;
using System.Net.NetworkInformation;

namespace Sandwych.SmartConfig
{
    public interface ISmartConfigDevice
    {
        PhysicalAddress MacAddress { get; }
        IPAddress IPAddress { get; }
    }
}
