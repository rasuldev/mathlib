using mathlib.Functions;
using System;

namespace mathlib
{
    public static class WalshLinearCombination
    {
        public static double Calc(double[] alpha, double x)
        {
            var s = 0d;
            for (int k = 0; k < alpha.Length; k++)
            {
                s += alpha[k] * Walsh.Get(k)(x);
            }
            return s;
        }
    }
}
