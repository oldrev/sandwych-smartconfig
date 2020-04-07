using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Sandwych.SmartConfig.Protocol
{
    public interface IDevicePacketInterpreter
    {
        bool Validate(SmartConfigContext context, byte[] packet);

        PhysicalAddress ParseMacAddress(byte[] packet);
    }
}
