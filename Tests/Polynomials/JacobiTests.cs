using System;
using mathlib;
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

        [Test]
        public void GetTest()
        {
            var jacobi = new Jacobi(0, 0); // Legendre case
            var func0 = jacobi.Get(0);
            Assert.That(func0(0.5), Is.EqualTo(jacobi.GetOrthonormalValue(0, 0.5)).Within(1e-10));

            var func1 = jacobi.Get(1);
            Assert.That(func1(0.5), Is.EqualTo(jacobi.GetOrthonormalValue(1, 0.5)).Within(1e-10));

            var func2 = jacobi.Get(2);
            Assert.That(func2(0.5), Is.EqualTo(jacobi.GetOrthonormalValue(2, 0.5)).Within(1e-10));

            // Test with different alpha, beta
            var jacobi2 = new Jacobi(0.5, 0.5);
            var func0_2 = jacobi2.Get(0);
            Assert.That(func0_2(-0.5), Is.EqualTo(jacobi2.GetOrthonormalValue(0, -0.5)).Within(1e-10));

            var func1_2 = jacobi2.Get(1);
            Assert.That(func1_2(-0.5), Is.EqualTo(jacobi2.GetOrthonormalValue(1, -0.5)).Within(1e-10));
        }

        [Test]
        public void OrthogonalityAndNormingTest()
        {
            var jacobi = new Jacobi(1, 1);
            var funcs = new Func<double, double>[5];
            for (int i = 0; i < 5; i++)
            {
                funcs[i] = jacobi.Get(i);
            }

            int nodesCount = 10000;
            double a = -1, b = 1;

            for (int m = 0; m < 5; m++)
            {
                for (int n = 0; n < 5; n++)
                {
                    Func<double, double> integrand = x => funcs[m](x) * funcs[n](x) * (1 - x * x);
                    double integral = Integrals.Trapezoid(integrand, a, b, nodesCount);

                    if (m == n)
                    {
                        Assert.That(integral, Is.EqualTo(1.0).Within(1e-6));
                    }
                    else
                    {
                        Assert.That(integral, Is.EqualTo(0.0).Within(1e-6));
                    }
                }
            }
        }

        [Test]
        public void IntegralRelationTest()
        {
            // Test the formula: ∫_{-1}^x \hat{P}_n^{0,0}(t) dt = -√(1/(n(n+1))) (1-x²) \hat{P}_{n-1}^{1,1}(x)
            var legendre = new Jacobi(0, 0); // \hat{P}_n^{0,0}
            var jacobi11 = new Jacobi(1, 1); // \hat{P}_n^{1,1}

            int nodesCount = 10000; // for numerical integration

            for (int n = 1; n <= 4; n++) // n from 1 to 4
            {
                var legendre_n = legendre.Get(n);
                var jacobi11_n = jacobi11.Get(n - 1);

                // Test at several points
                double[] testPoints = { -0.8, -0.5, 0.0, 0.5, 0.8 };

                foreach (double x in testPoints)
                {
                    // Numerical integral ∫_{-1}^x Pn(t) dt
                    double integral = Integrals.Trapezoid(legendre_n, -1, x, nodesCount);

                    // Right-hand side
                    double rhs = -Sqrt(1.0 / (n * (n + 1))) * (1 - x * x) * jacobi11_n(x);

                    Assert.That(integral, Is.EqualTo(rhs).Within(1e-5));
                }
            }
        }
    }
}
