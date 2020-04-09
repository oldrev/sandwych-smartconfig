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
            ctx.SetOption(AirkissOptionNames.RandomNumber, (byte)107);

            return ctx;
        }

        [Test]
        public void TestRealWorldCase()
        {
            var args = new SmartConfigArguments
            {
                Ssid = "MMKD",
                Bssid = new PhysicalAddress(new byte[] { 0xc4, 0x12, 0xf5, 0xc4, 0x92, 0x69 }),
                Password = "lixiaoxiong",
            };

            var context = this.CreateContext();
            context.SetOption(AirkissOptionNames.RandomNumber, (byte)186);
            var encoder = new AirkissProcedureEncoder();

            var segments = encoder.Encode(context, args);
            var frames = segments.SelectMany(x => x.Frames).ToArray();

            var expectedBuf = new ushort[] {
                1, 2, 3, 4, // Guide Code 0-3
                1, 16, 44, 51,	// Magic Code 4-7
                64, 91, 98, 112, // Prefix Code 8-11

                // Data
                144, 128, 364, 361, 376, 361,  // seq0
                145, 129, 353, 367, 376, 361,  // seq1
                160, 130, 367, 366, 359, 442,  // seq2
                141, 131, 333, 333, 331, 324   // seq3
            };

            Assert.AreEqual(expectedBuf.Length, frames.Length);

            for (int i = 0; i < expectedBuf.Length; i++)
            {
                Assert.AreEqual(expectedBuf[i], frames[i], "Failed index={0}", i);
            }
        }

        [Test]
        public void TestEmptyPasswordRealWorldCase()
        {
            var args = new SmartConfigArguments
            {
                Ssid = "MMKD",
                Bssid = new PhysicalAddress(new byte[] { 0xc4, 0x12, 0xf5, 0xc4, 0x92, 0x69 }),
            };

            var context = this.CreateContext();
            context.SetOption(AirkissOptionNames.RandomNumber, (byte)67);
            var encoder = new AirkissProcedureEncoder();

            var segments = encoder.Encode(context, args);
            var frames = segments.SelectMany(x => x.Frames).ToArray();

            var expectedBuf = new ushort[] {
                1, 2, 3, 4, // Guide Code 0-3
                8, 21, 44, 51, // Magic Code 4-7
                64, 80, 96, 112, // Prefix Code 8-11

                // Data
                189, 128, 323, 333, 333, 331,
                227, 129, 324 
            };

            //Assert.AreEqual(expectedBuf.Length, frames.Length);

            for (int i = 0; i < expectedBuf.Length; i++)
            {
                Assert.AreEqual(expectedBuf[i], frames[i], "Failed index={0}", i);
            }
        }

    }
}
