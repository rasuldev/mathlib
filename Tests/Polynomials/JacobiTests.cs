using System;
using mathlib.Polynomials;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using static System.Math;

namespace Tests.Polynomials
{
    public class JacobiTests
    {
        [Test]
        public void TchebCaseTest()
        {
            var t = new Jacobi(-0.5, -0.5);
            Assert.AreEqual(1, t.GetValue(0, 0.5));

            Assert.AreEqual(0.5, t.GetValue(1, 0.5));
            Assert.AreEqual(0, t.GetValue(1, 0));
            Assert.AreEqual(-0.5, t.GetValue(1, -0.5));


            Assert.That(!double.IsNaN(t.GetValue(2, -1)));

            Func<double, double> tcheb2 =
                x => (2 * Pow(x, 2) - 1) / 2;

            Func<double, double> tcheb6 =
                x => (32 * Pow(x, 6) - 48 * Pow(x, 4) + 18 * Pow(x, 2) - 1) / 32;

            var n = 100;
            var h = 2d / n;
            for (int i = 0; i < 100; i++)
            {
                var x = -1 + i * h;
                Assert.AreEqual(tcheb2(x), t.GetValue(2, x));
                Assert.AreEqual(tcheb6(x), t.GetValue(6, x), Assert.Eps);
            }

        }

        [Test]
        public void Tcheb2ndCaseTest()
        {
            var t = new Jacobi(0.5, 0.5);
            Func<double, double> tcheb2 =
                x => Pow(x, 2) - 0.25;

            Func<double, double> tcheb3 =
                x => Pow(x, 3) - 0.5 * x;

            Func<double, double> tcheb4 =
                x => Pow(x, 4) - 0.75 * Pow(x, 2) + 1d / 16;

            var n = 100;
            var h = 2d / n;
            for (int i = 0; i < 100; i++)
            {
                var x = -1 + i * h;
                Assert.AreEqual(x, t.GetValue(1, x));
                Assert.AreEqual(tcheb2(x), t.GetValue(2, x));
                Assert.AreEqual(tcheb3(x), t.GetValue(3, x), Assert.Eps);
                Assert.AreEqual(tcheb4(x), t.GetValue(4, x), Assert.Eps);
            }

        }

        [Test]
        public void CalcHTest()
        {
            // Test for Legendre case (alpha=0, beta=0)
            Assert.That(Jacobi.CalcH(0, 0, 0), Is.EqualTo(2.0).Within(1e-10));
            Assert.That(Jacobi.CalcH(1, 0, 0), Is.EqualTo(2.0 / 3.0).Within(1e-10));

            // Test for another case (alpha=0.5, beta=0.5)
            double expected = Jacobi.CalcH(0, 0.5, 0.5);
            Assert.That(expected, Is.Not.NaN);
            Assert.That(expected, Is.Not.EqualTo(double.PositiveInfinity));
            Assert.That(expected, Is.GreaterThan(0));

            // Test the formula h_{n-1}^{1,1} = 8 / (2n + 1) * n / (n + 1)
            for (int n = 1; n <= 5; n++)
            {
                double expectedH = 8.0 / (2 * n + 1) * n / (n + 1);
                Assert.That(Jacobi.CalcH(n - 1, 1, 1), Is.EqualTo(expectedH).Within(1e-10));
            }
        }
    }
}
