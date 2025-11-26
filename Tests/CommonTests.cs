using System.Linq;
using mathlib;
using NUnit.Framework;

namespace Tests
{
    public class CommonTests
    {
        [Test]
        public void FactorialTest()
        {
            Assert.That(Common.Factorial(5), Is.EqualTo(120));
        }

        [Test]
        public void ToBinaryTest()
        {
            var bin = Common.ToBinary(1);
            Assert.AreEqual(1, bin.Length);
            Assert.AreEqual(1, bin[0]);


            bin = Common.ToBinary(2);
            Assert.AreEqual(2, bin.Length);
            Assert.AreEqual(new[] { 0, 1 }, bin);

            bin = Common.ToBinary(3);
            Assert.AreEqual(2, bin.Length);
            Assert.AreEqual(new[] { 1, 1 }, bin);


            bin = Common.ToBinary(150);
            Assert.AreEqual(8, bin.Length);
            Assert.AreEqual(new[] { 1, 0, 0, 1, 0, 1, 1, 0 }.Reverse().ToArray(), bin);
        }

        [Test]
        public void ToBinaryDoubleTest()
        {
            var bin = Common.ToBinary(0.625, 3);
            Assert.AreEqual(new[] { 1, 0, 1 }, bin);
        }

        [Test]
        public void DecomposeTest()
        {
            var n = 10;
            var (k, i) = Common.Decompose(n);
            Assert.AreEqual(3, k);
            Assert.AreEqual(2, i);

            n = 8;
            (k, i) = Common.Decompose(n);
            Assert.AreEqual(2, k);
            Assert.AreEqual(4, i);
        }

        [Test]
        public void GetClosestPowerOf2Test()
        {
            var n = 10;
            var k = Common.GetClosestPowerOf2(n);
            Assert.That(k, Is.EqualTo(4));

            n = 15;
            k = Common.GetClosestPowerOf2(n);
            Assert.That(k, Is.EqualTo(4));

            n = 16;
            k = Common.GetClosestPowerOf2(n);
            Assert.That(k, Is.EqualTo(5));
        }
    }
}