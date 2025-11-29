using System;
using static System.Math;

namespace mathlib.Polynomials
{
    public static class LegendreSobolev
    {
        /// <summary>
        /// Legendre - Sobolev polynomial with $r=1$.
        /// </summary>
        /// <param name="k">k >= 0</param>
        /// <returns></returns>
        public static Func<double, double> Get(int k)
        {
            if (k == 0)
                return x => 1;

            return x => (1-x*x);
        }
    }
}