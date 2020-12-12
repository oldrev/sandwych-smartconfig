using Sandwych.SmartConfig.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Sandwych.SmartConfig.Esptouch.Protocol
{
    public sealed class EspDevicePacketInterpreter : IDevicePacketInterpreter
    {
        public PhysicalAddress ParseMacAddress(byte[] packet)
        {
            var macSpan = new ArraySegment<byte>(packet, 1, 6);
            return new PhysicalAddress(macSpan.ToArray());
        }

        public bool Validate(SmartConfigContext context, byte[] packet)
        {
            return packet.Length == EspWellKnownConstants.EspDevicePacketLength && packet[0] == EspWellKnownConstants.EspDevicePacketMagic;
        }
    }
}
