using System;
using System.Collections.Generic;

namespace Sandwych.SmartConfig.Esptouch
{
    public static class EspWellKnownConstants
    {

        public static IReadOnlyList<ushort> GuideCodes { get; } = new ushort[] { 515, 514, 513, 512 };

        public const String EspTouchVersion = "v0.3.7.2";

        public const byte EspDevicePacketMagic = 0x18;
        public const int EspDevicePacketLength = 11;

    }
}
