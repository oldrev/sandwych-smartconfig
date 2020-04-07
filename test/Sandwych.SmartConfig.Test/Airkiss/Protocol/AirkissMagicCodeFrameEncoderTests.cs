using NUnit.Framework;
using Sandwych.SmartConfig.Airkiss.Protocol;
using Sandwych.SmartConfig.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandwych.SmartConfig.Test.Airkiss.Protocol
{
    public class AirkissMagicCodeFrameEncoderTests
    {

        [Test]
        public void EncodingShouldBeOk()
        {
            var buf = Encoding.ASCII.GetBytes("MMKD");

            var crc = Crc8.ComputeOnceOnly(buf, 0, buf.Length);
            var magicCodeFrames = AirkissMagicCodeFrameEncoder.Encode(16, crc);
            var frames = magicCodeFrames.ToArray();

            Assert.AreEqual(1, frames[0]);
            Assert.AreEqual(16, frames[1]);
            Assert.AreEqual(44, frames[2]);
            Assert.AreEqual(51, frames[3]);
        }
    }
}
