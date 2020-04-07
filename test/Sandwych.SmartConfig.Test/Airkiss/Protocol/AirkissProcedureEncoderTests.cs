using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandwych.SmartConfig.Util;
using Sandwych.SmartConfig.Airkiss.Protocol;
using Sandwych.SmartConfig.Airkiss;
using System.Net.NetworkInformation;
using System.Net;

namespace Sandwych.SmartConfig.Test.Airkiss.Protocol
{
    public class AirkissProcedureEncoderTests
    {

        private SmartConfigContext CreateContext()
        {
            var provider = new AirkissSmartConfigProvider();
            var ctx = provider.CreateContext();
            ctx.SetOption(AirkissProperties.RandomNumber, (byte)107);

            return ctx;
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
        public void CheckByCapturedData()
        {
            var context = this.CreateContext();
            var encoder = new AirkissProcedureEncoder();

            var segments = encoder.Encode(context, this.CreateArguments());
            var frames = segments.SelectMany(x => x.Frames).ToArray();

            var expectedBuf = new ushort[] {
                9, 10, 11, 12, // Guide Code 0-3
                9, 24, 52, 59,	// Magic Code 4-7
                72,	99,	106, 120, // Prefix Code 8-11

                // Data
                152,136,372,369,384,369, // 9-14
                153,137,361,375,384,369,
                177,138,375,374,367,371,
                149,139,341,341,339,332	 //29-32
            };

            Assert.AreEqual(expectedBuf.Length, frames.Length);

            for (int i = 0; i < expectedBuf.Length; i++)
            {
                Assert.AreEqual(expectedBuf[i] - 8, frames[i], "Failed index={0}", i);
            }
        }

    }
}
