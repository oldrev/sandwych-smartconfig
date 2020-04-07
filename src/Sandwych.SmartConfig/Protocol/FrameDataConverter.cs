using System;
using System.Collections.Generic;

namespace Sandwych.Esptouch.Protocol
{
    public static class FrameDataConverter
    {

        public static IReadOnlyList<byte> ValueToFilledByteArray(int value)
        {
            if (value > 0x1FF || value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            var result = new List<byte>(value);
            for (int i = 0; i < value; i++)
            {
                result[i] = (byte)'1';
            }
            return result;
        }

    }
}
