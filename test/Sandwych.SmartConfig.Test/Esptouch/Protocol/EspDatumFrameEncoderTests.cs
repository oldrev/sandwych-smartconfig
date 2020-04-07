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
            var result = EspDatumFrameEncoder.ByteToFrames(0x14, 0x4D);
            Assert.AreEqual(0xE4, result.Item1);
            Assert.AreEqual(0x114, result.Item2);
            Assert.AreEqual(0xED, result.Item3);

            result = EspDatumFrameEncoder.ByteToFrames(0x0C, 0x69);
            Assert.AreEqual(0x46, result.Item1);
            Assert.AreEqual(0x10C, result.Item2);
            Assert.AreEqual(0xB9, result.Item3);
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
