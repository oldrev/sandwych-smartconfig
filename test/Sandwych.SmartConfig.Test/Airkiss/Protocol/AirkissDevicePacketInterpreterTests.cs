using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using NUnit.Framework;
using Sandwych.SmartConfig.Airkiss;
using Sandwych.SmartConfig.Airkiss.Protocol;

namespace Sandwych.SmartConfig.Test.Airkiss.Protocol
{
    public class AirkissDevicePacketInterpreterTests
    {

        [Test]
        public void CanParseMacAddress()
        {
            var interpreter = new AirkissDevicePacketInterpreter();
            var packet = new byte[] { 0x53, 0xc8, 0x2b, 0x96, 0xa1, 0x57, 0x70 };
            var provider = new AirkissSmartConfigProvider();
            var ctx = provider.CreateContext();
            ctx.SetOption<byte>(AirkissProperties.RandomNumber, 0x53);
            Assert.True(interpreter.Validate(ctx, packet));
            var mac = interpreter.ParseMacAddress(packet);
            var expectedMac = new PhysicalAddress(new byte[] { 0xc8, 0x2b, 0x96, 0xa1, 0x57, 0x70 });
            Assert.AreEqual(expectedMac, mac);
        }
    }
}
