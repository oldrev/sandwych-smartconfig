using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandwych.SmartConfig.Util;
using Sandwych.SmartConfig.Airkiss.Protocol;

namespace Sandwych.SmartConfig.Test.Airkiss.Protocol
{
    public class AirkissSeqEntryFrameEncoderTests
    {

        [Test]
        public void EncodingSeqEntryShouldBeOk()
        {
            var buf = Encoding.ASCII.GetBytes("lixi");
            var frames = AirkissSeqEntryFrameEncoder.Encode(0, buf).ToArray();

            var expectedBuf = new ushort[] {
                152, 136, 372, 369, 384, 369
            };

            Assert.AreEqual(6, frames.Count());

            for (int i = 0; i < expectedBuf.Length; i++)
            {
                Assert.AreEqual(expectedBuf[i] - 8, frames[i]);
            }
        }

        [Test]
        public void EncodingSeqEntryEnsureIndexCrcIsGood()
        {
            var buf = Encoding.ASCII.GetBytes("aoxi");
            var frames = AirkissSeqEntryFrameEncoder.Encode(1, buf).ToArray();

            var expectedBuf = new ushort[] {
                145, 129, 353, 367, 376, 361, // aoxi
            };

            Assert.AreEqual(6, frames.Count());

            for (int i = 0; i < expectedBuf.Length; i++)
            {
                Assert.AreEqual(expectedBuf[i], frames[i]);
            }
        }

        [Test]
        public void EncodingSeqEntryLongBufferShouldBeOk()
        {
            var passwordBuf = Encoding.ASCII.GetBytes("lixiaoxiong");
            var ssidBuf = Encoding.ASCII.GetBytes("MMKD");
            var buf = new List<byte>();
            buf.AddRange(passwordBuf);
            buf.Add(1);
            buf.AddRange(ssidBuf);

            var frames = new List<ushort>();
            int index = 0;
            foreach(IEnumerable<byte> part in buf.Partition(4))
            {
                var segFrames = AirkissSeqEntryFrameEncoder.Encode(index, part);
                frames.AddRange(segFrames);
                index++;
            }

            var expectedBuf = new ushort[]
            {
                152, 136, 372, 369, 384, 369, // lixi
                153, 137, 361, 375, 384, 369, // aoxi
                186, 138, 375, 374, 367, 265, // ong\1
                149, 139, 341, 341, 339, 332, // MMKD
            };


            for (int i = 0; i < expectedBuf.Length; i++)
            {
                var expectedValue = expectedBuf[i] - 8;
                Assert.AreEqual(expectedValue, frames[i], 
                    "Verfication failed: expected={0}, actual={1}, index={2}", expectedValue, frames[i], i);
            }
        }
    }
}
