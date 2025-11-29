using System;
using mathlib.Symbolic;

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
    }

}