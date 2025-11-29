using System;
using static System.Math;

namespace mathlib.Polynomials
{
    
    public class Jacobi
    {
        private readonly double _alpha;
        private readonly double _beta;
        public Jacobi(double alpha, double beta)
        {
            _alpha = alpha;
            _beta = beta;
        }

        /// <summary>
        /// Jacobi polynom with the highest coeff equal to 1
        /// Using formulas from Натансон. Конструктивная теория функций (p. 411)
        /// </summary>
        public double GetValue(int n, double x)
        {
            if (n == 0) return 1;
            if (n == 1) return x + (_alpha - _beta) / (_alpha + _beta + 2);

            return (x - CalcAlphaCoeff(n - 2, _alpha, _beta)) * GetValue(n - 1, x) -
                   CalcLambdaCoeff(n - 2, _alpha, _beta) * GetValue(n - 2, x);
        }

        /// <summary>
        /// Returns normed Jacobi polynomials
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Func<double,double> Get(int n)
        {
            return x => GetOrthonormalValue(n, x);
        }

        public Func<double, double> GetStandartized(int n)
        {
            double h_n = CalcH(n, _alpha, _beta);
            double factor = 1.0 / Sqrt(h_n);
            return x => factor * GetValue(n, x);    
        }

        public double GetOrthonormalValue(int n, double x)
        {
            var s = _alpha + _beta + 2 * n + 1;
            var coeff = Sqrt(
                alglib.gammafunction(s) / alglib.gammafunction(s - n - _alpha) / alglib.gammafunction(s - _beta - n)
                * alglib.gammafunction(s) / alglib.gammafunction(s - n) / alglib.gammafunction(n + 1)
                * s / Pow(2, s)
            );
            return coeff * GetValue(n, x);
        }

        /// <summary>
        /// Calculates \alpha_{n+2} from recurrence relation
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        static double CalcAlphaCoeff(int n, double alpha, double beta) =>
            (beta * beta - alpha * alpha) / (alpha + beta + 2 * n + 2) / (alpha + beta + 2 * n + 4);

        /// <summary>
        /// Calculates \lambda_{n+1} from recurrence relation
        /// </summary>
        /// <param name="n"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public static double CalcLambdaCoeff(int n, double alpha, double beta)
        {

            if (n == 0)
            {

                return (alpha + 1) / (alpha + beta + 2)
                        * (beta + 1) / (alpha + beta + 2)
                        * 4 / (alpha + beta + 3);
            }


            var t = alpha + beta + 2 * n + 1;
            return (alpha + n + 1) / t
                    * (beta + n + 1) / (t + 1)
                    * (alpha + beta + n + 1) / (t + 1)
                    * 4 * (n + 1) / (t + 2);



        }

        /// <summary>
        /// Calculates h_{n}^{\alpha,\beta} = \frac{2^{\alpha+\beta+1}}{2n+\alpha+\beta+1} \frac{\Gamma(n+\alpha+1)\Gamma(n+\beta+1)}{\Gamma(n+1)\Gamma(n+\alpha+\beta+1)}
        /// </summary>
        /// <param name="n">Degree of the polynomial</param>
        /// <param name="alpha">Alpha parameter</param>
        /// <param name="beta">Beta parameter</param>
        /// <returns>The calculated h value</returns>
        public static double CalcH(int n, double alpha, double beta)
        {
            double num = Pow(2, alpha + beta + 1);
            double den = 2 * n + alpha + beta + 1;
            double gamma1 = alglib.gammafunction(n + alpha + 1);
            double gamma2 = alglib.gammafunction(n + beta + 1);
            double gamma3 = alglib.gammafunction(n + 1);
            double gamma4 = alglib.gammafunction(n + alpha + beta + 1);
            return num / den * gamma1 * gamma2 / (gamma3 * gamma4);
        }

    }
}
