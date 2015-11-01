using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trazable.Engine.Base;
using Trazable.Engine.Utils;

namespace Trazable.Engine.Tests
{
    [TestClass]
    public class AngleTest
    {
        [TestMethod]
        public void TestAngleCreate()
        {
            Angle angle = new Angle(0);
            Assert.IsNotNull(angle);
            Assert.AreEqual(0, angle.Normalization);
            Assert.AreEqual(0, angle.Degree);
            Assert.AreEqual(0, angle.Grad);
            Assert.AreEqual(0, angle.Radian);
        }

        [TestMethod]
        public void TestAngleDegree()
        {
            Angle angle = new Angle(47.83);
            Assert.AreEqual(47.83, angle.Degree);
            angle.Degree = -1234.56;
            Assert.AreEqual(-1234.56, angle.Degree);
        }

        [TestMethod]
        public void TestAngleGrad()
        {
            Angle angle = new Angle(0);
            Assert.AreEqual(0, angle.Grad);
            angle.Grad = -1234.56;
            Assert.AreEqual(-1234.56, angle.Grad);
        }

        [TestMethod]
        public void TestAngleRadian()
        {
            Angle angle = new Angle(0);
            Assert.AreEqual(0, angle.Radian);
            angle.Radian = -1234.56;
            Assert.AreEqual(-1234.56, angle.Radian);
        }

        [TestMethod]
        public void TestConversions()
        {
            Angle angle = new Angle(45);
            double expected = 200 / 4;
            Assert.AreEqual(expected, angle.Grad);
            expected = MathUtil.RemoveDecimals(Math.PI / 4);
            Assert.AreEqual(expected, angle.Radian);
            angle.Degree = 90;
            expected = 200 / 2;
            Assert.AreEqual(expected, angle.Grad);
            expected = MathUtil.RemoveDecimals(Math.PI / 2);
            Assert.AreEqual(expected, angle.Radian);
            angle.Degree = 135;
            expected = 200*3 / 4;
            Assert.AreEqual(expected, angle.Grad);
            expected = MathUtil.RemoveDecimals(Math.PI*3 / 4);
            Assert.AreEqual(expected, angle.Radian);
            angle.Degree = 180;
            expected = 200;
            Assert.AreEqual(expected, angle.Grad);
            expected = MathUtil.RemoveDecimals(Math.PI);
            Assert.AreEqual(expected, angle.Radian);
            angle.Degree = -45;
            expected = -200 / 4;
            Assert.AreEqual(expected, angle.Grad);
            expected = MathUtil.RemoveDecimals(-Math.PI / 4);
            Assert.AreEqual(expected, angle.Radian);
        }

        [TestMethod]
        public void TestNormalization()
        {
            Angle angle = new Angle(405, 360);
            Assert.AreEqual(45, angle.Degree);
            double expected = 200 / 4;
            Assert.AreEqual(expected, angle.Grad);
            expected = MathUtil.RemoveDecimals(Math.PI / 4);
            Assert.AreEqual(expected, angle.Radian);
            angle.Degree = -315;
            Assert.AreEqual(45, angle.Degree);
            angle.Degree = 270;
            Assert.AreEqual(270, angle.Degree);
            angle.Normalization = 180;
            Assert.AreEqual(90, angle.Degree);
        }

    }
}
