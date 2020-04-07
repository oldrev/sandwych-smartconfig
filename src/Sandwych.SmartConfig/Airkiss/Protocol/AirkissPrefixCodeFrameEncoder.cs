using Sandwych.SmartConfig.Protocol;
using Sandwych.SmartConfig.Util;
using System.Collections.Generic;

namespace Sandwych.SmartConfig.Airkiss.Protocol
{
    public static class AirkissPrefixCodeFrameEncoder 
    {
        public static IEnumerable<ushort> Encode(int passwordLength)
        {
            var frames = new ushort[4];
            var blen = (byte)passwordLength;
            var lenCrc8 = Crc8.ComputeOnceOnly(blen);
            frames[0] = BytesHelper.CombineUshort(0x04, blen.Bisect().high);
            frames[1] = BytesHelper.CombineUshort(0x05, blen.Bisect().low);
            frames[2] = BytesHelper.CombineUshort(0x06, lenCrc8.Bisect().high);
            frames[3] = BytesHelper.CombineUshort(0x07, lenCrc8.Bisect().low);
            return frames;
        }
    }
}
