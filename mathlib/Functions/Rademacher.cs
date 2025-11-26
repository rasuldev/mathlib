using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathlib.Functions
{
    public class Rademacher
    {

        public static Func<double, double> Get(int n)
        {
            double R0(double x)
            {
                x = x - Math.Truncate(x);
                if (x < 0.5) return 1;
                return -1;
            }

            var pwr2n = Math.Pow(2, n);
            return x => R0(pwr2n * x);
        }
    }
}
