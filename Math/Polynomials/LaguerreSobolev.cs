using System;
using static System.Math;

namespace mathlib.Polynomials
{
    public class LaguerreSobolev
    {
        /// <summary>
        /// Laguerre - Sobolev polynomial with $\alpha=0$.
        /// </summary>
        /// <param name="r">r >= 1</param>
        /// <param name="k">k >= 0</param>
        /// <returns></returns>
        public static Func<double, double> Get(int r, int k)
        {
            if (k < r)
                return x => Pow(x, k) / Common.Factorial(k);

            var mul = 1;
            k -= r;
            for (int j = 1; j <= r; j++)
            {
                mul *= k + j;
            }

            return x => Pow(x, r) * Laguerre.Calc(r, k, x) / mul;
        }
    }
}