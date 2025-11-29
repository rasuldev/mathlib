using System;
using mathlib.Symbolic;
using static System.Math;

namespace mathlib
{
    public class InnerProd
    {
        public static Integral WeightedLebesgue(Func<double, double> f, Func<double, double> g,
            Func<double, double> weight)
        {
            return new Integral(x => f(x)*g(x)*weight(x), 0, double.PositiveInfinity);
        }

        /// <summary>
        /// Scalar mul of Lebesgue space with weight $e^{-x}$. Laguerre polynomials with $\alpha=0$ are orthogonal in this space.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Integral LebesgueLaguerre(Func<double, double> f, Func<double, double> g)
        {
            return WeightedLebesgue(f, g, x => Math.Exp(-x));
        }

        /// <summary>
        /// Computes the Sobolev inner product ⟨f,g⟩ = f(a)g(a) + ∫_a^b f'(x)g'(x)(1-x)^α(1+x)^β dx
        /// </summary>
        /// <param name="f">Function f</param>
        /// <param name="g">Function g</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <param name="alpha">Alpha parameter for weight</param>
        /// <param name="beta">Beta parameter for weight</param>
        /// <param name="h">Step size for numerical differentiation (default 1e-5)</param>
        /// <param name="nodesCount">Number of nodes for numerical integration (default 1000)</param>
        /// <returns>The value of the inner product</returns>
        public static double SobolevJacobi(Func<double, double> f, Func<double, double> g, double a, double b, double alpha, double beta, double h = 1e-5, int nodesCount = 1000)
        {
            // Boundary term
            double boundary = f(a) * g(a);

            // Derivative functions
            Func<double, double> fPrime = x => (f(x + h) - f(x - h)) / (2 * h);
            Func<double, double> gPrime = x => (g(x + h) - g(x - h)) / (2 * h);

            // Integrand: f'(x)g'(x)(1-x)^α(1+x)^β
            Func<double, double> integrand = x => fPrime(x) * gPrime(x) * Pow(1 - x, alpha) * Pow(1 + x, beta);

            // Numerical integration
            double integral = Integrals.Trapezoid(integrand, a, b, nodesCount);

            return boundary + integral;
        }

        public static double SobolevJacobi(Func<double, double> f, Func<double, double> g, double alpha, double beta, double h = 1e-5, int nodesCount = 1000)
        {
            return SobolevJacobi(f, g, -1, 1, alpha, beta, h, nodesCount);
        }

        public static double SobolevLegendre(Func<double, double> f, Func<double, double> g, double h = 1e-5, int nodesCount = 1000)
        {
            // Boundary term
            double boundary = f(-1) * g(-1);

            // Derivative functions
            Func<double, double> fPrime = x => (f(x + h) - f(x - h)) / (2 * h);
            Func<double, double> gPrime = x => (g(x + h) - g(x - h)) / (2 * h);

            // Integrand: f'(x)g'(x)(1-x)^α(1+x)^β
            Func<double, double> integrand = x => fPrime(x) * gPrime(x);

            // Numerical integration
            double integral = Integrals.Trapezoid(integrand, -1, 1, nodesCount);

            return boundary + integral;
        }
    }

}
