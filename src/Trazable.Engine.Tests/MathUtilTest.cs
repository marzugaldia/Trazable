using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trazable.Engine.Utils;

namespace Trazable.Engine.Tests
{
    /// <summary>
    /// Summary description for MathUtilTest
    /// </summary>
    [TestClass]
    public class MathUtilTest
    {
        [TestMethod]
        public void TestAlmostEqual()
        {
            Assert.IsFalse(MathUtil.AlmostEqual(1000, 2000, -3));
            Assert.IsFalse(MathUtil.AlmostEqual(1000, 1500, -3));
            Assert.IsTrue(MathUtil.AlmostEqual(1000, 1499, -3));
            Assert.IsTrue(MathUtil.AlmostEqual(3, 3.499, 0));
            Assert.IsFalse(MathUtil.AlmostEqual(3, 3.5, 0));
            Assert.IsTrue(MathUtil.AlmostEqual(1.234567800001, 1.2345678000012, 12));
            Assert.IsFalse(MathUtil.AlmostEqual(1.234567800001, 1.234567800001501, 12));
        }
    }
}
