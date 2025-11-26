using mathlib.Functions;
using System;

namespace mathlib
{
    public static class SobolevWalshLinearCombination
    {
        public static double Calc(double[] alpha, double x)
        {
            var s = 0d;
            for (int k = 0; k < alpha.Length; k++)
            {
                s += alpha[k] * WalshSobolev.Get2(k + 1)(x);
            }
            return s;
        }
    }
}
