using System;
using System.Collections.Generic;
using System.Text;

namespace Sandwych.SmartConfig.Airkiss
{
    public class AirkissWellknownConstants
    {
        public static IReadOnlyList<ushort> GuideCodes { get; } = new ushort[] { 1, 2, 3, 4 };
        public const int DevicePacketLength = 7;
    }
}
