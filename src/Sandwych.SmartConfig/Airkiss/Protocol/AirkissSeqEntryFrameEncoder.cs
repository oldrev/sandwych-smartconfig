using Sandwych.SmartConfig.Protocol;
using Sandwych.SmartConfig.Util;
using System.Collections.Generic;
using System.Linq;

namespace Sandwych.SmartConfig.Airkiss.Protocol
{
    public static class AirkissSeqEntryFrameEncoder 
    {
        public static IEnumerable<ushort> Encode(int index, IEnumerable<byte> bytes)
        {
            var frames = new List<ushort>(2 + bytes.Count());

            var crc = new Crc8();
            crc.Update((byte)(index & 0x7F));
            crc.Update(bytes);

            frames.Add((ushort)(0x80 | (crc.Value & 0x7F)));
            frames.Add((ushort)(0x80 | index));

            foreach (var b in bytes)
            {
                frames.Add((ushort)(0x100 | b));
            }
            return frames;
        }

    }
}
