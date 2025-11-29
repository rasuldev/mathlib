using System;

namespace mathlib
{
    /// <summary>
    /// Newton binomial.
    /// </summary>
    public class Binomial
    {
        public static int Calc(int n, int k)
        {
            if (k == 0 || k == n)
            {
                return 1;
            }

            if (k < 0 || k > n || n < 0)
                throw new ArgumentOutOfRangeException();

            return Calc(n - 1, k - 1) + Calc(n - 1, k);
        }
    }
}