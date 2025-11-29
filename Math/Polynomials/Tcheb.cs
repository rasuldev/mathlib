using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace mathlib.Polynomials
{
    public class Tcheb
    {
        /// <summary>
        /// Returns normed Tchebyshev polynomials
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Func<double, double> Get(int n)
        {
            if (n == 0) return x => 1 / Sqrt(PI);
            return x => Sqrt(2 / PI) * GetStandartized(n)(x);
        }

        public static Func<double, double> GetStandartized(int n)
        {
            if (n == 0) return x => 1;
            if (n == 1) return x => x;

            return x => 2 * x * GetStandartized(n - 1)(x) - GetStandartized(n - 2)(x);
        }
    }
}
