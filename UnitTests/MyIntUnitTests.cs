using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MyIntProject
{
    [TestFixture]
    class MyIntUnitTests
    {
        [Test]
        public void testCreate()
        {
            Assert.AreEqual((new MyInt(123)).ToString(), "123");
            Assert.AreEqual((new MyInt(123)).longValue(), 123);

            Assert.AreEqual((new MyInt(-123)).ToString(), "-123");
            Assert.AreEqual((new MyInt(-123)).longValue(), -123);

            Assert.AreEqual((new MyInt("123")).ToString(), "123");
            Assert.AreEqual((new MyInt("123")).longValue(), 123);

            Assert.AreEqual((new MyInt("-123")).ToString(), "-123");
            Assert.AreEqual((new MyInt("-123")).longValue(), -123);

            Assert.AreEqual((new MyInt(new int[] { 0, 1, 2, 3 })).ToString(), "123");
            Assert.AreEqual((new MyInt(new int[] { 0, 1, 2, 3 })).longValue(), 123);

            Assert.AreEqual((new MyInt(new int[] { 1, 1, 2, 3 })).ToString(), "-123");
            Assert.AreEqual((new MyInt(new int[] { 1, 1, 2, 3 })).longValue(), -123);

            Assert.AreEqual((new MyInt(0)).ToString(), "0");
            Assert.AreEqual((new MyInt(0)).longValue(), 0);

            Assert.AreEqual((new MyInt(-0)).ToString(), "0");
            Assert.AreEqual((new MyInt(-0)).longValue(), 0);

            Assert.AreEqual((new MyInt("0")).ToString(), "0");
            Assert.AreEqual((new MyInt("0")).longValue(), 0);

            Assert.AreEqual((new MyInt("-0")).ToString(), "0");
            Assert.AreEqual((new MyInt("-0")).longValue(), 0);

            Assert.AreEqual((new MyInt(new int[] { 0, 0 })).ToString(), "0");
            Assert.AreEqual((new MyInt(new int[] { 0, 0 })).longValue(), 0);

            Assert.AreEqual((new MyInt(new int[] { 1, 0 })).ToString(), "0");
            Assert.AreEqual((new MyInt(new int[] { 1, 0 })).longValue(), 0);
        }

        [Test]
        public void testCompareTo()
        {
            Assert.IsTrue((new MyInt(new int[] { 1, 1, 2 })).compareTo(new MyInt(-12)));
            Assert.IsTrue((new MyInt("-0")).compareTo(new MyInt(new int[] { 0, 0, 0, 0 })));
            Assert.IsTrue((new MyInt(4213)).compareTo(new MyInt("04213")));
            Assert.IsFalse((new MyInt(130)).compareTo(new MyInt(new int[] { 1, 1, 3, 0 })));
        }

        [Test]
        public void testAbs()
        {
            Assert.AreEqual((new MyInt(new int[] { 1, 1, 3 })).abs().longValue(), 13);
            Assert.AreEqual((new MyInt("-0250")).abs().longValue(), 250);
            Assert.AreEqual((new MyInt(520)).abs().longValue(), 520);
            Assert.AreEqual((new MyInt("-12")).abs().longValue(), 12);
        }

        [Test]
        public void testMaxMin()
        {
            MyInt a = new MyInt(200);
            MyInt b = new MyInt(199);
            MyInt c = new MyInt("201");
            MyInt d = new MyInt(-200);

            Assert.AreEqual(b.max(a), a.max(b));
            Assert.AreEqual(a.max(b), a);
            Assert.AreEqual(c.max(a), c);
            Assert.AreEqual(c.max(b), c);
            Assert.AreEqual(b.max(d), b);

            Assert.AreEqual(b.min(d), d.min(b));
            Assert.AreEqual(a.min(b), b);
            Assert.AreEqual(a.min(c), a);
            Assert.AreEqual(d.min(a), d);
            Assert.AreEqual(c.min(d), d);            
        }
    }
}
