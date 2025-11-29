using mathlib;
using mathlib.Polynomials;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class LegendreTests
    {
           
        [Test]
        public void LegendrePolynomialTest()
        {
            for (int n = 0; n < 10; n++)
            {
                var f = Legendre.Get(n);
                var norm = Integrals.Trapezoid(x => f(x) * f(x), -1, 1, 1024);
                Assert.That(norm, Is.EqualTo(1).Within(0.001));
            }

        }
    }
}
