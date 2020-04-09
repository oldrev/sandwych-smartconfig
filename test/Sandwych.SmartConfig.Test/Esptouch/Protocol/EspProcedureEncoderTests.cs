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
    public class EspProcedureEncoderTests
    {

        private SmartConfigContext CreateContext()
        {
            var provider = new EspSmartConfigProvider();
            return provider.CreateContext();
        }

        [Test]
        public void TestRealWorldCase()
        {
            var args = new SmartConfigArguments
            {
                Ssid = "MMKD",
                Bssid = new PhysicalAddress(new byte[] { 0xc4, 0x12, 0xf5, 0xc4, 0x92, 0x69 }),
                Password = "123456",
                LocalAddress = IPAddress.Parse("192.168.1.6")
            };
            var ctx = this.CreateContext();
            var encoder = new EspProcedureEncoder();
            var segments = encoder.Encode(ctx, args);
            var allFrames = segments.SelectMany(x => x.Frames).ToArray();
            var expectedFrames = new ushort[] 
            {
                515, 514, 513, 512, 
                217, 296, 187,      280, 297, 110,      132, 298, 251,      234, 299, 114,      177, 300, 67, 
                292, 315, 44,       180, 301, 216,      114, 302, 128, 
                104, 303, 153,      169, 316, 58,       136, 304, 174, 
                155, 305, 121,      235, 306, 74,       247, 317, 109, 
                123, 307, 171,      219, 308, 124,      75, 309, 285, 
                244, 318, 284,      187, 310, 174,      124, 311, 101,      
                172, 312, 293,      113, 319, 234,      156, 313, 227, 
                172, 314, 60,       222, 320, 161
            };

            Assert.AreEqual(expectedFrames.Length, allFrames.Count());
            for (int i = 0; i < expectedFrames.Length; i++)
            {
                Assert.AreEqual(expectedFrames[i], allFrames[i], 
                    "Failed to compare frames: index={0}, expected={1}, actual={2}",
                    i, expectedFrames[i], allFrames[i]);
            }


        }


        [Test]
        public void TestEmptyPasswordCase()
        {
            var args = new SmartConfigArguments
            {
                Ssid = "MMKD",
                Bssid = new PhysicalAddress(new byte[] { 0xc4, 0x12, 0xf5, 0xc4, 0x92, 0x69 }),
                Password = null,
                LocalAddress = IPAddress.Parse("192.168.1.6")
            };
            var ctx = this.CreateContext();
            var encoder = new EspProcedureEncoder();
            var segments = encoder.Encode(ctx, args);
            var allFrames = segments.SelectMany(x => x.Frames).ToArray();
            var expectedFrames = new ushort[] 
            {
                515, 514, 513, 512, //guide
                168, 296, 197, 120, 297, 264, 132, 298, 251, 234, 299, 114, 160, 300, 124, // datum
                164, 309, 76,       180, 301, 216,      114, 302, 128,      104, 303, 153,      137, 310, 74,
                136, 304, 174,      172, 305, 277,      140, 306, 245,      87, 311, 157,       188, 307, 179,
                44, 308, 92,        68, 312, 76,        193, 313, 58,       238, 314, 193
            };

            Assert.AreEqual(expectedFrames.Length, allFrames.Count());
            for (int i = 0; i < expectedFrames.Length; i++)
            {
                Assert.AreEqual(expectedFrames[i], allFrames[i], 
                    "Failed to compare frames: index={0}, expected={1}, actual={2}",
                    i, expectedFrames[i], allFrames[i]);
            }


        }
    }
}
