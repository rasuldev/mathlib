using System;
using mathlib;
using NUnit.Framework;

namespace Tests
{
    public class FixedPointIterationTests
    {
        [Test]
        public void FindTest()
        {
            var result = FixedPointIteration.FindFixedPoint((double x) => 0.5 * (9.0 / x + x), 1, 30);
            Assert.That(3, Is.EqualTo(result).Within(0.01));
        }

        [Test]
        public void FindWithEpsTest()
        {
            var result = FixedPointIteration.FindFixedPoint((double x) => 0.5 * (9.0 / x + x), 1, 0.001, (x,y) => Math.Abs(x-y), 1000);
            Assert.That(3, Is.EqualTo(result).Within(0.001));
        }
    }
}