using System;

namespace mathlib.Polynomials
{
    public class Laguerre
    {
        /// <summary>
        /// Laguerre polynomials with \alpha=0. Used explicit formula.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<double, double> Get(int n)
        {
            var coeffs = Coeffs(n);
            return x =>
            {
                var sum = coeffs[0];
                for (int k = 1; k < coeffs.Length; k++)
                {
                    sum += coeffs[k] * x;
                    x *= x;
                }
                return sum;
            };
        }

        /// <summary>
        /// Coefficients for Laguerre polynomials with \alpha=0.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double[] Coeffs(int n)
        {
            var coeffs = new double[n + 1];
            var mul = 1d;
            coeffs[0] = mul;
            for (int k = 1; k < coeffs.Length; k++)
            {
                mul *= -1.0 / k;
                coeffs[k] = mul * Binomial.Calc(n, k);
            }
            return coeffs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="n"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Calc(double alpha, int n, double x)
        {
            if (n == 0) return 1;
            if (n == 1) return -x + alpha + 1;

            return (-x + 2*n + alpha - 1)/n*Calc(alpha, n - 1, x) - (n + alpha - 1)/n*Calc(alpha, n - 2, x);
        }
    }
}