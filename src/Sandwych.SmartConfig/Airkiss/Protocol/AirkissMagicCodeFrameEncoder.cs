using Sandwych.SmartConfig.Protocol;
using Sandwych.SmartConfig.Util;
using System.Collections.Generic;

namespace Sandwych.SmartConfig.Airkiss.Protocol
{
    public static class AirkissMagicCodeFrameEncoder
    {

        public static IEnumerable<ushort> Encode(int totalLength, byte ssidCrc)
        {
            ushort[] frames = new ushort[4];
            var blen = (byte)totalLength;
            var firstFrame = BytesHelper.CombineUshort(0x00, blen.Bisect().high);
            frames[0] = firstFrame != 0 ? firstFrame : (ushort)0x08;
            frames[1] = BytesHelper.CombineUshort(0x01, blen.Bisect().low);
            frames[2] = BytesHelper.CombineUshort(0x02, ssidCrc.Bisect().high);
            frames[3] = BytesHelper.CombineUshort(0x03, ssidCrc.Bisect().low);
            return frames;
        }
    }
}
