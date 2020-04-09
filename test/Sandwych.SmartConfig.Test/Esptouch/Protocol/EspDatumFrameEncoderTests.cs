using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Sandwych.SmartConfig.Esptouch.Protocol;
using Sandwych.SmartConfig.Esptouch;
using Sandwych.SmartConfig;
using System.Net.NetworkInformation;

namespace Sandwych.Esptouch.Test.Esptouch.Protocol
{
    public class EspDatumFrameEncoderTests
    {

        private SmartConfigContext CreateContext()
        {
            var provider = new EspSmartConfigProvider();
            return provider.CreateContext();
        }

        private SmartConfigArguments CreateArguments()
        {
            return new SmartConfigArguments
            {
                Ssid = "MMKD",
                Bssid = new PhysicalAddress(new byte[] { 0xc4, 0x12, 0xf5, 0xc4, 0x92, 0x69 }),
                Password = "lixiaoxiong",
                LocalAddress = IPAddress.Parse("192.168.1.11")
            };
        }

        [Test]
        public void ByteToPacketFramesShouldBeOk()
        {
            var result = EspDatumFrameEncoder.ByteToFrames(0x14, 0x4D).ToArray();
            Assert.AreEqual(268, result[0]);
            Assert.AreEqual(316, result[1]);
            Assert.AreEqual(277, result[2]);

            result = EspDatumFrameEncoder.ByteToFrames(0x0C, 0x69).ToArray();
            Assert.AreEqual(110, result[0]);
            Assert.AreEqual(308, result[1]);
            Assert.AreEqual(225, result[2]);
        }

        [Test]
        public void FramesMustMatchPrecomputed()
        {
            var ctx = this.CreateContext();
            var encoder = new EspDatumFrameEncoder();
            var frames = encoder.Encode(ctx, this.CreateArguments());

            var headerFrames = new ushort[]
            {
                185, 296, 208, 152, 297, 259, 132, 298, 251, 234, 299, 114, 71, 300, 290
            };

            // Make sure the datum's header match
            Assert.That(headerFrames.SequenceEqual(frames.Take(headerFrames.Length)));
        }

    }
}
