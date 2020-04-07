using Sandwych.SmartConfig.Protocol;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Sandwych.SmartConfig.Airkiss.Protocol
{
    public class AirkissDevicePacketInterpreter : IDevicePacketInterpreter
    {
        public PhysicalAddress ParseMacAddress(byte[] packet)
        {
            var span = new ReadOnlySpan<byte>(packet);
            var macSpan = span.Slice(1, 6);
            return new PhysicalAddress(macSpan.ToArray());
        }

        public bool Validate(SmartConfigContext context, byte[] packet)
        {
            var randomValue = context.GetOption<byte>(AirkissProperties.RandomNumber);
            return (packet.Length == AirkissWellknownConstants.DevicePacketLength && packet[0] == randomValue);
        }
    }
}
