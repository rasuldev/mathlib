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


            Assert.IsFalse(double.IsNaN(t.GetValue(2, -1)));

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
    }
}