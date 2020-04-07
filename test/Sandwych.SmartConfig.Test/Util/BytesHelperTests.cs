using NUnit.Framework;
using Sandwych.SmartConfig.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sandwych.SmartConfig.Test.Util
{
    public class BytesHelperTests
    {
        [Test]
        public void CombineUshortShouldWorks()
        {
            Assert.AreEqual((ushort)0x43, BytesHelper.CombineUshort(0x4, 0x3));
        }

        [Test]
        public void BisectUshortShouldWorks()
        {
            Assert.AreEqual(0x9, BytesHelper.Bisect(0x98).high);
            Assert.AreEqual(0x8, BytesHelper.Bisect(0x98).low);
            Assert.AreEqual(0x3, BytesHelper.Bisect(0x43).low);
        }
    }
}
