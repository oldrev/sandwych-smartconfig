using NUnit.Framework;
using System.Text;

using Sandwych.SmartConfig.Util;
using System.Linq;

namespace Sandwych.Esptouch.Test.Util
{
    public class Crc8Tests
    {
        [Test]
        public void CheckCrcTable()
        {
            Assert.AreEqual(256, Crc8.Table.Count);

            Assert.AreEqual(0x0, Crc8.Table.First());
            Assert.AreEqual(0x5E, Crc8.Table[1]);
            Assert.AreEqual(0xC2, Crc8.Table[8]);
            Assert.AreEqual(0x35, Crc8.Table.Last());
        }

        [Test]
        public void ComputeOnceShouldWork()
        {
            Assert.AreEqual(0xC3, Crc8.ComputeOnceOnly(Encoding.UTF8.GetBytes("MMKD")));

            var buf = new byte[2] { 0x0B, 0x01 };
            Assert.AreEqual(0x7D, Crc8.ComputeOnceOnly(buf));
        }

        [Test]
        public void UpdateShouldWork()
        {
            var crc = new Crc8();

            crc.Reset();
            var buf = new byte[2] { 0x0B, 0x01 };
            Assert.AreEqual(0x7D, crc.Update(buf));
            Assert.AreEqual(0x7D, crc.Value);

            crc.Reset();
            buf = new byte[2] { 0xC3, 0x02 };
            crc.Update(buf[0]);
            crc.Update(buf[1]);
            Assert.AreEqual(0x5D, crc.Value);
        }
    }
}