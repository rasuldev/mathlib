using mathlib;
using mathlib.Polynomials;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class LegendreTests
    {

        [Test]
        public void LegendreNormTest()
        {
            for (int n = 0; n < 10; n++)
            {
                var f = Legendre.Get(n);
                var norm = Integrals.Trapezoid(x => f(x) * f(x), -1, 1, 1024);
                Assert.That(norm, Is.EqualTo(1).Within(0.001));
            }

        }

        [Test]
        public void LegendreOrthogonalityTest()
        {
            for (int n = 0; n < 10; n++)
            {
                for (int m = n + 1; m < 10; m++)
                {
                    var fn = Legendre.Get(n);
                    var fm = Legendre.Get(m);
                    var dot = Integrals.Trapezoid(x => fn(x) * fm(x), -1, 1, 1024);
                    Assert.That(dot, Is.EqualTo(0).Within(0.001));
                }
            }
        }
    }
}
