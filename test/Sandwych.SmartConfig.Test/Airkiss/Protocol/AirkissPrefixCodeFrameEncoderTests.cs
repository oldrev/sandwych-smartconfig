using NUnit.Framework;
using Sandwych.SmartConfig.Airkiss.Protocol;
using Sandwych.SmartConfig.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandwych.SmartConfig.Test.Airkiss.Protocol
{
    public class AirkissPrefixCodeFrameEncoderTests
    {

        [Test]
        public void EncodingShouldBeOk()
        {
            var buf = Encoding.ASCII.GetBytes("lixiaoxiong");
            var frames = AirkissPrefixCodeFrameEncoder.Encode(buf.Length).ToArray();

            Assert.AreEqual(72 - 8, frames[0]);
            Assert.AreEqual(99 - 8, frames[1]);
            Assert.AreEqual(106 - 8, frames[2]);
            Assert.AreEqual(120 - 8, frames[3]);
        }
    }
}
