using mathlib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class SobolevWalshLinearCombinationTests
    {
        [Test]
        public void LinearCombinationTest()
        {
            var alpha = new double[] { 1, 1, 1 };
            var result = SobolevWalshLinearCombination.Calc(alpha, 0);
            Assert.That(result, Is.EqualTo(0));

            result = SobolevWalshLinearCombination.Calc(alpha, 0);
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
