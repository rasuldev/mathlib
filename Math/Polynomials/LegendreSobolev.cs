using System;
using static System.Math;

namespace mathlib.Polynomials
{
    public static class LegendreSobolev
    {
        /// <summary>
        /// Legendre - Sobolev orthonormed polynomial with $r=1$.
        /// </summary>
        /// <param name="n">n >= 0</param>
        /// <returns></returns>
        public static Func<double, double> Get(int n)
        {
            if (n == 0) return x => 1;
            if (n == 1) return x => Sqrt(1d / 2) * (x + 1);

            // P_{1,n}(x)=\int_{-1}^{x}\hat{P}_{n-1}(t)dt=-\sqrt{\frac{1}{n(n-1)}}(1-x^{2})\hat{P}_{n-2}^{1,1}(x)
            var jacobi11 = new Jacobi(1, 1);
            var jacobi11_n_minus_two = jacobi11.Get(n - 2);
            return x => -Sqrt(1.0 / (n * (n - 1))) * (1 - x * x) * jacobi11_n_minus_two(x);
        }
    }
}